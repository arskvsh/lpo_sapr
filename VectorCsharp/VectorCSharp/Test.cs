using System;

namespace VectorCSharp
{
    public class Test
    {
        static void Main(string[] args)
        {
            ArsVector<int> vector = new ArsVector<int>();
            if (vector != null)
                Console.WriteLine("Вектор создан!");

            vector.Append(25);

            Console.WriteLine(vector[0]);

            Console.WriteLine(vector.Length());

            Console.ReadLine();
        }
    }
}
