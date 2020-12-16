﻿using System;

namespace LPO_SAPR
{
    public class Test
    {
        static void Main(string[] args)
        {
            //инициализируем вектор целочисленного типа с индексом первого элемента 0 и последнего 9
            ArsVector<int> vector = new ArsVector<int>(0, 9);

            //проверяем, что вектор создался
            if (vector != null)
                Console.WriteLine("Вектор с индексом первого элемента 0 и последнего 9 создан!");

            //пробуем заменить созданный вектор, задав индекс первого 9 и последнего 0.
            //мы реализовали вывод ошибки в данном случае, поэтому добавим обработчик исключений
            try
            {
                vector = new ArsVector<int>(9, 0);
            }
            catch
            {
                Console.WriteLine("Вектор с индексом первого 9 и последнего 0 создать нельзя!");
            }

            //выведем на экран длину вектора
            Console.WriteLine("Длина вектора: " + vector.Length());
            //выведем на экран индекс первого элемента
            Console.WriteLine(vector.GetFirstIndex());
            //выведем на экран индекс последнего элемента
            Console.WriteLine(vector.GetLastIndex());

            //заполним вектор возрастающей последовательностью чисел, кратных 3 и выведем их на экран
            for (int i = 0; i < vector.Length(); i++)
            {
                vector[i] = (i + 1) * 3;
                Console.Write(vector[i] + " ");
            }

            //заменим элемент с индексом 5 и выведем его на экран
            vector[5] = 65576;
            Console.WriteLine("\nЗаменили элемент 5 на " + vector[5]);

            Console.WriteLine();

            Console.ReadLine();
        }
    }
}
