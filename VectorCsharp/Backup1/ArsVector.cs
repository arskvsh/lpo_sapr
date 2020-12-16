using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VectorCSharp
{
    static class DEF
    {
        public const int SIZE = 5; //константа для хранения стандартного размера
        public const int SIZE_MULT = 2; //константа для хранения множителя увеличения памяти
    }

    //объявление класса. Ключевое слово unsafe необходимо для работы с указателями на память.
    //тип пока что неопределённый T, т.к. будет задан уже при объявлении конкретного вектора.
    public unsafe class ArsVector<T> where T : unmanaged
    {
        //ДЕСКРИПТОР ВЕКТОРА
        private T* data; //указатель на поле для хранения данных вектора неопределённого типа. Заданный в дальнейшем тип определит и длину элемента.
        private int m; //индекс первого элемента
        private int n; //индекс последнего элемента

        private int size; //техническое поле для хранения размера памяти, которая зарезервирована под вектор

        //конструктор класса
        public ArsVector()
        {
            fixed (T* setData = new T[DEF.SIZE]) //фиксируем ссылку на участок памяти длиной DEF.SIZE (резервируем память под вектор)
            {
                data = setData;
                size = DEF.SIZE;
                n = -1;
            }
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index > size)
                {
                    throw new IndexOutOfRangeException();
                }
            }

            set
            {

            }
        }

        public void Append()
        {

        }

        static void Main(string[] args)
        {
            Console.Write("Hello world!");
        }
    }
}
