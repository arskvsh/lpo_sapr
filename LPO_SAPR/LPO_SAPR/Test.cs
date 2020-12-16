using System;

namespace LPO_SAPR
{
    public class Test
    {
        static void Main(string[] args)
        {
            ArsVector<int> vector = new ArsVector<int>(0, 9);
            if (vector != null)
                Console.WriteLine("Вектор создан!");

            for (int i = 0; i < vector.Length(); i++)
                vector[i] = (i + 1)*3;


            for (int i = 0; i < vector.Length(); i++)
                Console.WriteLine(vector[i]);

            Console.WriteLine("Длина: {0}", vector.Length());

            Console.ReadLine();
        }
    }
}
