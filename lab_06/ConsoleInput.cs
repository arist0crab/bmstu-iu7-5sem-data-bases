using System;
using System.IO.Pipelines;
using System.Net;

namespace Lab06.ConsoleInputModule
{
    public static class ConsoleInput
    {
        public static uint GetUint()
        {
            uint uint_num = 0;
            bool inputCycle = true;

            while (inputCycle)
            {
                string? input = Console.ReadLine();

                if (inputCycle = !uint.TryParse(input, out uint_num))
                {
                    Console.WriteLine("[ ConsoleInput ] Ошибка ввода целого неотрицательного числа");
                    Console.Write("Попробуйте еще раз: ");
                }
            }

            return uint_num;
        }

        public static uint GetUintRange(uint min, uint max)
        {
            uint uint_num = GetUint();
            bool inputCycle = true;

            while (inputCycle)
            {
                if (inputCycle = min > uint_num || max < uint_num)
                {
                    Console.WriteLine("[ ConsoleInput ] Ошибка ввода целого неотрицательного числа: число не лежит в заданном диапазоне");
                    Console.Write("Попробуйте еще раз: ");
                    uint_num = GetUint();
                }
            }

            return uint_num;
        }

        public static string GetString()
        {
            string? input = Console.ReadLine();

            while (input == null)
            {
                Console.Write("[ ConsoleInput ] Введите непустую строку: ");
                input = Console.ReadLine();
            }

            return input;
        }

        public static DateTime GetDateTime()
        {
            DateTime result = DateTime.Now;
            bool inputCycle = true;

            while (inputCycle)
            {
                string? input = Console.ReadLine();

                if (DateTime.TryParse(input, out result))
                {
                    inputCycle = false;
                }
                else
                {
                    Console.Write("[ ConsoleInput ] Неверный формат даты. Попробуйте еще раз: ");
                }
            }

            return result;
        }
    }
}