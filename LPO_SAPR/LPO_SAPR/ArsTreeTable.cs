using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace LPO_SAPR
{
    public unsafe class ArsTreeTable<T> where T : unmanaged
    {
        //В C# записи могут быть представлены структурой "структура"
        //Создадим запись для элемента древовидной таблицы
        public struct Record
        {
            private int lptr; //левый указатель - индекс массива
            private T keyvalue; //поле ключ+данные, у меня будет строкового типа
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

        private Record[] records; //поле хранения записей
        //private object[] recordz; //поле хранения записей

        //конструктор древовидной таблицы
        public ArsTreeTable(int _length)
        {
            //если заданное количество свободных элементов меньше нуля, то выбрасываем ошибку
            if (_length < 1)
                throw new Exception("Нельзя создать древовидную таблицу нулевой длины!");

            //подсчитаем полную длину таблицы с учётом места под технические записи
            int total_length = _length + 3;

            records = new Record[total_length]; //инициализируем массив

            //обнуляем все левые указатели и опустошаем поля данных
            for (int i = 0; i < total_length; i++)
            {
                records[i].L = 0;
                records[i].Key = default;
            }

            records[0].L = 2; //задаём указатель (индекс) на корень дерева
            records[0].R = 2; //задаём указатель (индекс) на список свободных элементов
            records[1].L = 0; //задаём счётчик количества существующих элементов
            records[1].R = _length; //задаём размер таблицы
        }

        //объявим индексатор для доступа к элементам
        public Record this[int index]
        {
            //действии при получении данных из вектора по индексу
            get
            {
                //выполняем проверку на выход индекса за границы массива
                if (index < 0 || index > records[0].R - 1)
                    throw new IndexOutOfRangeException();

                return records[index + 2];
            }

            //действии при задании данных в вектор по индексу
            set
            {
                //выполняем проверку на выход индекса за границы вектора, если выход обнаружен, выбрасываем ошибку, прерывающую программу
                if (index < 0 || index > records[0].R - 1)
                    throw new IndexOutOfRangeException();

                //использовано выражение, аналогичное описанному выше, только теперь значение задаётся
                records[index + 2] = value;
            }
        }

        public void Add(T Key, int L, int R)
        {

        }

        private void Search(T searchKey)
        {
            for (int i = 0; i < records[1].R; i++)
            {
                if (searchKey.Equals(this[i].Key))
                {

                } 
                else
                {

                }
            }
        }
    }
}
