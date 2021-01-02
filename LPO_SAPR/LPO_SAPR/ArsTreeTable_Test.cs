using System;
using System.Text;

namespace LPO_SAPR
{
    public class ArsTreeTable_Test
    {
        static void Main(string[] args)
        {
            //инициализируем древовидную таблицу с 10 свободными слотами под записи и данными целочисленного типа.
            ArsTreeTable<int> treetable = new ArsTreeTable<int>(10);

            treetable[0].L = 1;

        }
    }
}