using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel.Design.Serialization;

namespace LPO_SAPR
{
    public unsafe class ArsTreeTable<T> where T : unmanaged
    {
        //В C# записи могут быть представлены структурой "структура"
        //Создадим запись для элемента древовидной таблицы
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

        private Record[] table; //поле хранения записей
        //private object[] recordz; //поле хранения записей

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

            table[0].L = 2; //задаём указатель (индекс) на корень дерева
            table[0].R = 2; //задаём указатель (индекс) на список свободных элементов
            table[1].L = 0; //задаём счётчик количества существующих элементов
            table[1].R = _length; //задаём размер таблицы
        }

        //объявим индексатор для доступа к элементам извне
        public Record this[int index]
        {
            //действия при получении данных по индексу
            get
            {
                //выполняем проверку на выход индекса за границы массива
                if (index < 0 || index > table[0].R - 1)
                    throw new IndexOutOfRangeException();

                return table[index + 2];
            }

            //действии при задании данных по индексу
            set
            {
                //выполняем проверку на выход индекса за границы массива
                if (index < 0 || index > table[0].R - 1)
                    throw new IndexOutOfRangeException();

                //использовано выражение, аналогичное описанному выше, только теперь значение задаётся
                table[index + 2] = value;
            }
        }

        public void Add(T Key)
        {
            int search = Search(Key, table[0].L);

            if (search != -1)
            {
                if (search > 0)
                    table[search - 1].L = search;

                table[0].R = table[search].R;

                table[search].Key = Key;
                table[search].R = table[0].R;
                table[search].L = table[0].R;

                table[1].L += 1;
            } else
            {
                Console.WriteLine("Элемент с таким ключом уже существует!");
            }
        }

        private int Search(T searchKey, int rootIndex)
        {
            int result = -1;

            if(searchKey.Equals(table[rootIndex].Key))
            {
                return rootIndex;
            } 
            else
            {
                Search(searchKey, table[rootIndex].L);
                Search(searchKey, table[rootIndex].R);
            }

            return result;
        }

        private int NodeState(int index)
        {
            if ()
        }
    }
}
