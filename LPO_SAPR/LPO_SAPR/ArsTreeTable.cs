using System;
using System.IO;

namespace LPO_SAPR
{
    //объявляем класс древовидной таблицы
    //требование к типу при объявлении - IComparable (сравнимый), т.к. в бинарном дереве поиска необходимо производить сравнение ключей
    public unsafe class ArsTreeTable<T> where T : IComparable
    {
        //В C# записи могут быть представлены структурой "структура"
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
            for (int i = 0; i < total_length - 1; i++)
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

            //действие при задании данных по индексу
            //поскольку изменения полей вручную, минуя управляющие алгоритмы таблицы, может нарушить работу таблицы, запрещаем их изменение
            set
            {
                throw new AccessViolationException();
            }
        }

        //свойство, возвращающее вместимость таблицы
        public int Capacity
        {
            get
            {
                return table[1].R - 3;
            }
        }

        //свойство, возвращающее количество занятых элементов таблицы
        public int Count
        {
            get
            {
                return table[1].L;
            }
        }

        //публичная функция добавления элемента
        public void Add(T Key)
        {
            if (table[1].L < table[1].R - 3)
            {
                //если таблица не заполнена
                if (table[1].L > 0)
                {
                    //запускаем рекурсивную функцию поиска и добавления нового элемента
                    //если возвращено -1, то выбрасываем сообщение, что элемент с таким ключом уже есть
                    if (Add_R(Key, table[0].L, table[0].R) == -1)
                        Console.WriteLine("Элемент с ключом {0} уже есть!", Key);
                }
                //если пуста, без проверок создаём корень дерева
                else
                    AddToTable(Key, table[0].R);
            } else
            {
                Console.WriteLine("Таблица переполнена!");
            }
        }

        //рекурсивная функция поиска места для указателя на новый элемент
        private int Add_R(T Key, int rootIndex, int j)
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
                    Add_R(Key, table[rootIndex].L, j);
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
                    Add_R(Key, table[rootIndex].R, j);
            }
            return 1;
        }

        //функция, производящая специфические для работы с таблицей операции при добавлении нового элемента
        private void AddToTable(T Key, int j)
        {
            //если дерево пусто, обновляем указатель на корень дерева
            if (table[1].L == 0)
                table[0].L = j;

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
                //находим удаляемый элемент и его родителя, записываем в поля
                (int, int) srch = Search(Key);
                int j = srch.Item1;
                int p = srch.Item2;

                if (j == -1)
                {
                    Console.WriteLine("Такого элемента не существует!");
                } else
                {
                    //проверяем, сколько потомков у удаляемого элемента
                    if (table[j].R == 0 && table[j].L == 0)
                    {
                        //обнуляем указатель в родителе на текущий элемент
                        if (table[p].L == j)
                            table[p].L = 0;
                        else
                            table[p].R = 0;
                    } else
                    {
                        //если левый указатель не нуль, переносим указатель в родителя
                        if (table[j].L != 0)
                        {
                            if (table[p].L == j)
                                table[p].L = table[j].L;
                            else
                                table[p].R = table[j].L;
                        }
                        //если правый указатель не нуль, переносим указатель в родителя
                        if (table[j].R != 0)
                        {
                            if (table[p].L != j)
                                table[p].L = table[j].R;
                            else
                                table[p].R = table[j].R;
                        }
                    }

                    //производим служебные операции по удалению элемента из таблицы
                    RemoveFromTable(j, p);
                }
            }
            else
            {
                Console.WriteLine("Таблица пуста!");
            }
        }

        //функция, производящая специфические для работы с таблицей операции при удалении элемента
        private void RemoveFromTable(int j, int p)
        {
            //уменьшаем счётчик занятых элементов
            table[1].L -= 1;

            //переносим текущий указатель на список свободных элементов в указатель на следующий элемент
            table[j].R = table[0].R;

            //заносим индекс текущего элемента в указатель на список свободных элементов
            table[0].R = j;
        }

        //функция поиска, возвращающая индекс найденного элемента и индекс его родителя (понадобится при удалении элементов)
        private (int result, int parent) Search(T searchKey)
        {
            //если таблица не пуста, запускаем рекурсивный поиск начиная с корня дерева
            if (table[1].L > 0)
                return Search_R(searchKey, table[0].L, -1);
            //или возвращаем -1
            return (-1, -1);
        }

        //рекурсивная функция поиска по древовидной таблице
        private (int result, int parent) Search_R(T searchKey, int rootIndex, int prevIndex)
        {
            //если поданный индекс не нулевой, осуществляем возврат индекса, либо продолжение поиска по левому или правому поддереву
            if (rootIndex != 0)
            {
                if (searchKey.CompareTo(table[rootIndex].Key) == 0)
                {
                    return (rootIndex, prevIndex);
                }
                else if (searchKey.CompareTo(table[rootIndex].Key) < 0)
                {
                    return Search_R(searchKey, table[rootIndex].L, rootIndex);
                }
                else
                {
                    return Search_R(searchKey, table[rootIndex].R, rootIndex);
                }
            }
            //если ничего не найдено, возвращаем -1
            return (-1, -1);
        }

        //функция обхода дерева
        public void Traversal()
        {
            if (table[1].L > 0)
            {
                Console.WriteLine("Обход дерева:");
                Traversal_R(table[0].L);
            }
            else
                Console.WriteLine("\nТаблица пуста!");
        }

        //рекурсивная функция обхода дерева
        private void Traversal_R(int rootIndex)
        {
            //если поданный индекс не нулевой, выводим ключ и продолжаем обход по поддеревьям
            if (rootIndex != 0)
            {
                Console.WriteLine("Ключ " + table[rootIndex].Key);
                Traversal_R(table[rootIndex].L);
                Traversal_R(table[rootIndex].R);
            }
        }

        //функция сохранения в файл
        public void WriteToFile(string filename)
        {
            //создаём поток записи файла
            using (StreamWriter file = new StreamWriter(@"A:\hobby\vuz\lpo\" + filename + ".txt", false))
            {
                //сохраняем записи в файл построчно
                for (int i = 0; i < table[1].R; i++)
                {
                    file.WriteLine(table[i].L + " " + table[i].Key + " " + table[i].R);
                }
            }
        }

        //функция считывания из файла
        public void ReadFromFile(string filename)
        {
            //создаём поток чтения файла
            using (StreamReader file = new StreamReader(@"A:\hobby\vuz\lpo\" + filename + ".txt"))
            {
                //считываем длину таблицы из правого указателя первой строки в файле
                string firstStr = file.ReadLine();
                int length = Int32.Parse(file.ReadLine().Split(' ')[2]);

                //возвращаем буфер в начало
                file.DiscardBufferedData();
                file.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);

                //считываем все записи из файла и заносим их в основное поле таблицы
                Record[] records = new Record[length];
                for (int i = 0; i < length; i++)
                {
                    string[] rec = file.ReadLine().Split(' ');
                    records[i].L = Int32.Parse(rec[0]);
                    records[i].Key = (T)Convert.ChangeType(rec[1], typeof(T));
                    records[i].R = Int32.Parse(rec[2]);
                }
                table = records;
            }
        }

        //функция показа содержимого структуры в табличном виде
        public void Show()
        {
            Console.WriteLine("------");
            for (int i = 0; i < table[1].R; i++)
            {
                Console.WriteLine("[" + i + "] " + table[i].L + " " + table[i].Key + " " + table[i].R);
                if (i == 1)
                    Console.WriteLine("------");
            }
            Console.WriteLine("------");
        }
    }
}
