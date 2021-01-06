using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel.Design.Serialization;
using System.Security.Cryptography.X509Certificates;

namespace LPO_SAPR
{
    //объявляем класс древовидной таблицы
    //требование к типу при объявлении - IComparable (сравнимый), т.к. в бинарном дереве поиска нужно сравнение ключей
    public unsafe class ArsTreeTable<T> where T : IComparable
    {
        //В C# записи могут быть представлены структурой "структура"
        //Создадим запись для элемента древовидной таблицы, причём сериализуемую (чтобы потом записать в файл фрагмент памяти)
        [Serializable]
        public struct Record
        {
            private int lptr; //левый указатель - индекс массива
            private T keyvalue; //поле ключ+данные
            private int rptr; //правый указатель - индекс массива

            //интерфейс для обращения к полю хранения левого указателя
            public int L
            {
                get { return lptr; }
                set { lptr = value; }
            }

            //интерфейс для обращения к полю хранения правого указателя
            public int R
            {
                get { return rptr; }
                set { rptr = value; }
            }

            //интерфейс для обращения к полю хранения ключа+данных
            public T Key
            {
                get { return keyvalue; }
                set { keyvalue = value; }
            }
        }

        private Record[] table; //поле хранения массива записей

        //конструктор древовидной таблицы
        public ArsTreeTable(int _length)
        {
            //если заданное количество свободных элементов меньше нуля, то выбрасываем ошибку
            if (_length < 1)
                throw new Exception("Нельзя создать древовидную таблицу нулевой длины!");

            //подсчитаем полную длину таблицы с учётом места под технические записи
            int total_length = _length + 3;

            table = new Record[total_length]; //инициализируем массив

            //обнуляем все левые указатели и опустошаем поля данных, в правые указатели заносим индексы следующих строк
            for (int i = 0; i < total_length; i++)
            {
                table[i].L = 0;
                table[i].Key = default;
                table[i].R = i + 1;
            }

            table[0].R = 2; //задаём указатель (индекс) на список свободных элементов
            table[1].L = 0; //задаём счётчик количества существующих элементов
            table[1].R = total_length; //задаём размер таблицы
        }

        //объявим индексатор для доступа к записям в таблице извне
        public Record this[int index]
        {
            //действия при получении данных по индексу
            get
            {
                //выполняем проверку на выход индекса за границы массива
                if (index < 0 || index > table[1].R - 1)
                    throw new IndexOutOfRangeException();

                return table[index + 2];
            }

            //действии при задании данных по индексу
            //поскольку изменения полей вручную, минуя управляющие алгоритмы таблицы, может нарушить работу таблицы, запрещаем их изменение
            set
            {
                throw new Exception("У вас нет доступа к прямому изменению таблицы!");
            }
        }
        public void Add(T Key)
        {
            //если таблица не заполнена
            if (table[1].L < table[1].R - 2)
            {
                //ищем подходящее j в списке свободных элементов
                //т.к. при удалении элемента за ним могут следовать занятые, нужно проверять, пусты ли ключи в списке свободных элементов
                int j;
                for (j = table[0].R; j < table[1].R; j++)
                {
                    //проверяем, пуст ли ключ записи с индексом j, если да, выбираем это j
                    if (table[j].Key.Equals(default(T)))
                    {
                        break;
                    }
                }

                //если таблица не пуста, запускаем рекурсивное внесение элемента
                if (table[1].L > 0)
                {
                    //запускаем рекурсивную функцию поиска места для указателя на новый элемент
                    //если возвращено -1, то выбрасываем сообщение, что элемент с таким ключом уже есть
                    if (AddRecursive(Key, 2, j) == -1)
                        Console.WriteLine("Элемент с ключом {0} уже есть!", Key);
                }
                //если пуста, без проверок создаём корень дерева
                else
                    AddToTable(Key, j);
            } else
            {
                Console.WriteLine("Таблица переполнена!");
            }
        }

        //рекурсивная функция поиска места для указателя на новый элемент
        private int AddRecursive(T Key, int rootIndex, int j)
        {
            //если такой ключ уже есть, возвращаем -1
            if (Key.CompareTo(table[rootIndex].Key) == 0)
                return -1;
            //иначе обходим дерево и сравниваем ключи с вносимым, если ключ меньше, то
            else if (Key.CompareTo(table[rootIndex].Key) < 0)
            {
                //если у подходящего узла свободен указатель
                if (table[rootIndex].L == 0)
                {
                    //вносим указатель на строку j в найденном узле
                    table[rootIndex].L = j;
                    //и 
                    AddToTable(Key, j);
                }
                //иначе ищем место для указателя на j дальше
                else
                    AddRecursive(Key, table[rootIndex].L, j);
            }
            //аналогично для ситуации, когда вставляемый ключ больше обходимого
            else if (Key.CompareTo(table[rootIndex].Key) > 0)
            {
                if (table[rootIndex].R == 0)
                {
                    table[rootIndex].R = j;
                    AddToTable(Key, j);
                }
                else
                    AddRecursive(Key, table[rootIndex].R, j);
            }
            return 1;
        }

        //функция, производящая специфические для работы с таблицей операции при добавлении нового элемента
        private void AddToTable(T Key, int j)
        {
            //меняем указатель на список свободных элементов
            table[0].R = table[j].R;

            //записываем ключ и обнуляем указатели в строке j
            table[j].Key = Key;
            table[j].R = 0;
            table[j].L = 0;

            //увеличиваем счётчик занятых элементов
            table[1].L += 1;
        }

        //функция удаления элемента по ключу
        public void Remove(T Key)
        {
            if (table[1].L > 0)
            {
                int j = Search(Key);

                if (j == -1)
                {
                    Console.WriteLine("Такого элемента не существует!");
                } else
                {
                    if (table[j].R == 0 && table[j].L == 0)
                    {
                        RemoveFromTable(j);
                    }
                    if (table[j].L != 0)
                    {

                    }
                }
            }
            else
            {
                Console.WriteLine("Таблица пуста!");
            }
        }

        private void ReplacePtrRecursive(int rootIndex, int ptrToReplace, int newPtr)
        {
            //если поданный (возврат индекса, поиск по левому или правому поддереву)
            if (table[rootIndex].L != 0)
            {

            }
            if (table[rootIndex].R != 0)
            {

            }
        }

        private void RemoveFromTable(int j)
        {
            table[1].L -= 1;

            table[j].R = table[0].R;
            table[0].R = j;
            table[j].L = 0;
            table[j].Key = default;
        }

        public int Search(T searchKey)
        {
            //если таблица не пуста, запускаем рекурсивный поиск начиная с корня дерева
            if (table[1].L > 0)
                return SearchRecursive(searchKey, 2);
            //или возвращаем -1
            return -1;
        }

        //рекурсивная функция поиска по древовидной таблице
        private int SearchRecursive(T searchKey, int rootIndex)
        {
            //если поданный (возврат индекса, поиск по левому или правому поддереву)
            if (rootIndex != 0)
            {
                if (searchKey.CompareTo(table[rootIndex].Key) == 0)
                {
                    return rootIndex;
                }
                else if (searchKey.CompareTo(table[rootIndex].Key) < 0)
                {
                    return SearchRecursive(searchKey, table[rootIndex].L);
                }
                else if (searchKey.CompareTo(table[rootIndex].Key) > 0)
                {
                    return SearchRecursive(searchKey, table[rootIndex].R);
                }
            }

            return -1;
        }

        public void Show()
        {
            for (int i = 0; i < table[1].R; i++)
            {
                Console.WriteLine(table[i].L + " " + table[i].Key + " " + table[i].R);
                if(i == 1)
                    Console.WriteLine("------");
            }
        }

        public int Length
        {
            get
            {
                return table[1].R - 3;
            }
        }
    }
}
