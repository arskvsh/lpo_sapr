using System;
using System.Data.Common;
using System.Text;
using System.IO;

namespace LPO_SAPR
{
    public class ArsTreeTable_Test
    {
        static void Main(string[] args)
        {
            //инициализируем древовидную таблицу с 10 свободными местами под записи.
            ArsTreeTable<int> treetable = new ArsTreeTable<int>(10);

            //проверяем, что таблица создалась, заодно выведем её длину
            if (treetable != null)
                Console.WriteLine("Древовидная таблица создана! Длина: " + treetable.Capacity);

            //внесём несколько элементов
            Console.WriteLine("\nВнесём 8 элементов...");
            treetable.Add(5);
            treetable.Add(3);
            treetable.Add(10);
            treetable.Add(11);
            treetable.Add(6);
            treetable.Add(2);
            treetable.Add(1);
            treetable.Add(7);
            Console.WriteLine("Количество внесённых элементов: " + treetable.Count);

            //проведём обход дерева в таблице
            treetable.Traversal();

            //выведем таблицу в консоль
            Console.WriteLine("Выведем саму таблицу в консоль, чтобы пронаблюдать, как записывается информация, в т.ч. и служебная:");
            treetable.Show();
            Console.WriteLine("Можем сделать вывод, что элементы успешно добавлены, и служебные записи ведутся правильно: " +
                "\nкорень действительно находится по индексу 2, список свободнх элементов начинается с элемента 10, занято 8 записей и общая длина таблицы 13.");

            //попробуем переполнить таблицу
            Console.WriteLine("\nПопробуем переполнить таблицу...");
            treetable.Add(8);
            treetable.Add(9);
            treetable.Add(12);

            //попробуем удалить элементы
            Console.WriteLine("\nУдалим элементы с ключами 1 и 2...");
            treetable.Remove(1);
            treetable.Remove(2);
            //и обойдём дерево снова
            Console.WriteLine("...и снова проведём обход по дереву");
            treetable.Traversal();
            Console.WriteLine("...Данные элементы действительно удалены!");

            //попробуем добавить уже существующий элемент
            Console.WriteLine("\nПопробуем добавить уже существующий элемент и получим следующее сообщение:");
            treetable.Add(5);

            //запишем таблицу в файл
            Console.WriteLine("\nЗапишем нашу таблицу в файл...");
            treetable.WriteToFile("tablitsa");
            Console.WriteLine(File.Exists(@"A:\hobby\vuz\lpo\tablitsa.txt") ? "Файл tablitsa.txt успешно создан!" : "Файла не существует!");

            Console.WriteLine("\nСоздадим новое поле древовидной таблицы любой длины и считаем в него наш файл...");
            ArsTreeTable<int> treetable2 = new ArsTreeTable<int>(1);
            treetable2.ReadFromFile("tablitsa");

            Console.WriteLine("Проведём обход в новой таблице, считанной из файла:");
            treetable2.Traversal();
            Console.WriteLine("Эта таблица абсолютно аналогична первой, считывание осуществляется успешно.");

        }
    }
}