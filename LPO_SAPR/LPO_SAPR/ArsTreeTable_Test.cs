using System;
using System.Text;

namespace LPO_SAPR
{
    public class ArsTreeTable_Test
    {
        static void Main(string[] args)
        {
            //инициализируем древовидную таблицу с 10 свободными местами под записи.
            ArsTreeTable<int> treetable = new ArsTreeTable<int>(10);

            treetable.Add(1);

        }
    }
}