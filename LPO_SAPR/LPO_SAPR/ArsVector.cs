using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VectorCSharp
{
    //Реализация структуры хранения данных ВЕКТОР (динамический массив)
    //Арсений Ковешников, БИВТ-18-3
    //Кафедра АПД ИТКН НИТУ «МИСиС»

    //объявление класса. Ключевое слово unsafe необходимо для работы с указателями на память.
    //тип пока что неопределённый T, т.к. будет задан уже при объявлении конкретного вектора.
    public unsafe class ArsVector<T> where T : unmanaged
    {
        //ДЕСКРИПТОР ВЕКТОРА
        private T* data; //указатель на поле для хранения данных вектора неопределённого типа. Заданный в дальнейшем тип определит и длину элемента.
        private int m; //индекс первого элемента
        private int n; //индекс последнего элемента

        private int size; //техническое поле для хранения размера памяти, которая зарезервирована под вектор
        private const int DEF_SIZE = 5; //константа для задания стандартного размера резерва памяти под вектор
        private const int SIZE_MULT = 2; //константа для хранения множителя расширения резерва памяти

        //конструктор класса
        public ArsVector()
        {
            fixed (T* setData = new T[DEF_SIZE]) //фиксируем ссылку на участок памяти длиной DEF_SIZE (резервируем память под вектор)
            {
                size = DEF_SIZE; //запоминаем длину зарезервированного участка памяти
                data = setData; //вносим полученные данные по адресу поля для хранения данных
                m = 0; //задаём индекс первого элемента как 0
                n = -1; //задаём индекс последнего элемента как -1, поскольку вектор пока пустой
            }
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index > n)
                {
                    throw new IndexOutOfRangeException();
                }

                return data[index];
            }

            set
            {
                if (index < 0 || index > n)
                {
                    throw new IndexOutOfRangeException();
                }

                n = index;

                data[index] = value;
            }
        }

        private void expandSize()
        {
            fixed (T* setData = new T[size * SIZE_MULT]) //фиксируем ссылку на участок памяти длиной DEF_SIZE (резервируем память под вектор)
            {
                T* _data = data;
            }
        }

        public void Append(T input) //функция, добавляющая новый элемент в конец вектора
        {
            data[n++] = input;
        }

        public int Length()
        {
            return n + 1;
        }
    }
}
