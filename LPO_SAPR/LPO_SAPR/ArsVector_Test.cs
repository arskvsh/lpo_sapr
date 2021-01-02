using System;
using System.Text;

namespace LPO_SAPR
{
    public class ArsVector_Test
    {
        static void Start(string[] args)
        {
            //инициализируем вектор целочисленного типа с индексом первого элемента 0 и последнего 9
            ArsVector<int> vector = new ArsVector<int>(0, 9);

            //проверяем, что вектор создался
            if (vector != null)
                Console.WriteLine("Вектор с индексом первого элемента 0 и последнего 9 создан!");

            Console.WriteLine("");
            //пробуем заменить созданный вектор, задав индекс первого 9 и последнего 0.
            //мы реализовали вывод ошибки в данном случае, поэтому добавим обработчик исключений
            try
            {
                Console.WriteLine("Пробуем создать вектор с неверными индексами крайних элементов...");
                vector = new ArsVector<int>(9, 0);
            }
            catch
            {
                Console.WriteLine("Вектор с индексом первого 9 и последнего 0 создать нельзя!");
            }

            Console.WriteLine("");
            //попробуем достать элемент вне границ массива
            try
            {
                Console.WriteLine("А попробуем-ка достать элемент с индексом -50 за пределами массива...");
                Console.WriteLine(vector[-50]);
            }
            catch
            {
                Console.WriteLine("Неа, тоже нельзя!");
            }

            Console.WriteLine("");
            Console.WriteLine("Проверка методов в классе вектора:");
            //выведем на экран длину вектора
            Console.WriteLine("Длина вектора: " + vector.Length());
            //выведем на экран индекс первого элемента
            Console.WriteLine("Индекс первого элемента: " + vector.GetFirstIndex);
            //выведем на экран индекс последнего элемента
            Console.WriteLine("Индекс последнего элемента: " + vector.GetLastIndex);

            Console.WriteLine("");
            //заполним вектор возрастающей последовательностью чисел, кратных 3 и выведем их на экран
            Console.WriteLine("А вот и сам наш вектор:");
            for (int i = vector.GetFirstIndex; i < (vector.GetFirstIndex + vector.Length()); i++)
            {
                vector[i] = (i + 1) * 3;
                Console.Write(vector[i] + " ");
            }

            Console.WriteLine("");
            //заменим элемент с индексом 5 и снова выведем массив на экран
            Console.WriteLine("\nЗаменим значение элемента с индексом 5 на 30");
            vector[5] = 30;
            Console.WriteLine("Заменили элемент, посмотрим, как он выглядит в массиве:");
            for (int i = vector.GetFirstIndex; i < (vector.GetFirstIndex + vector.Length()); i++)
            {
                Console.Write(vector[i] + " ");
            }

            Console.WriteLine("\n");
            Console.WriteLine("Протестируем поиск. Попробуем найти первое включение элемента со значением 30...");
            //попробуем найти индекс первого включения элемента 30
            Console.WriteLine("Индекс первого включения 30 равен " + vector.IndexOf(30));
            Console.WriteLine("Попробуем найти индекс элемента, которого в массиве нет... Например, 1000");
            //попробуем найти элемент 1000. Такого в векторе нет, должен быть выведен минимальный int
            Console.WriteLine("Индекс первого включения 1000 равен " + vector.IndexOf(1000) + "... То бишь, нет его в массиве");

            //теперь создадим новый вектор типа char, запишем в него русский алфавит. 
            //он находится в диапазоне символов таблицы UTF-8 с 1040 до 1071. Проверим заодно и такие странные индексы.
            ArsVector<char> alphabet = new ArsVector<char>(1040, 1071);

            Console.WriteLine("");
            //проверим работу вектора с типом данных char (как пример)
            Console.WriteLine("Проверим работу вектора с каким-то другим типом данных. Например, char.");
            //выведем на экран подпись к выводимым данным
            Console.WriteLine("Русский алфавит:");

            //занесём буквы алфавита в массив и сразу же выведем
            for (int i = 1040; i <= 1071; i++)
            {
                alphabet[i] = System.Convert.ToChar(i);
                Console.Write(alphabet[i] + " ");
            }

            //выведем количество букв в русском алфавите судя по нашему массиву
            Console.WriteLine("\nВ русском алфавите " + alphabet.Length() + " буквы");
            Console.WriteLine("А должно быть 33... А почему у нас 32? А потому что Ё в таблице символов UTF-8 стоит особнячком!");

            Console.WriteLine("");
            //выводим сообщение об успешном тестировании
            Console.WriteLine("Тестирование успешно завершено!");
        }
    }
}