using System;
using System.Data.Common;
using System.Text;

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
            treetable.Add(5);
            treetable.Add(3);
            treetable.Add(10);
            treetable.Add(6);
            treetable.Add(2);
            treetable.Add(1);
            treetable.Add(7);


            //выведем таблицу в консоль
            treetable.Show();

            treetable.Add(5);

            treetable.Traversal();
        }
    }
}