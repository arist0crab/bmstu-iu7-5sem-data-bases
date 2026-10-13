using System;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;

namespace Lab06.MenuModule
{
    public class Menu
    {
        public const ushort MENU_OPTIONS_QUANTITY = 10;

        private static Dictionary<int, Action> _actions = [];

        public static void Initialize(Dictionary<int, Action> actions)
        {
            _actions = actions;
        }

        static public void PrintMenu()
        {
            Console.WriteLine(
                """
                ╔════════════════════════════════════════════════════════╗
                ║                       MAIN MENU                        ║
                ╠════════════════════════════════════════════════════════╣
                ║  1. Посчитать количество активных учеников в школе     ║
                ║  2. Найти всех учеников с активными справками          ║
                ║  3. Найти самого старшего ученика класса               ║
                ║  4. Вывести список всех таблиц базы данных             ║
                ║  5. Вывести количество пропусков ученика               ║
                ║  6. Вывести все парты и места в классе                 ║
                ║  7. Убрать всех неактивных учеников из класса          ║
                ║  8. Получить текущую версию сервера                    ║
                ║  9. Создать таблицу achiements                         ║
                ║ 10. Вставить тестовые данные в achiements              ║
                ║ 0. Выход                                               ║
                ╚════════════════════════════════════════════════════════╝
                """
            );
        }

        static public bool ExecuteMenuOption(ushort opt)
        {
            if (opt == 0)
                return false;

            if (_actions.TryGetValue(opt, out var action))
                action.Invoke();
            else
                Console.WriteLine("[ Menu ] Указанного пункта меню не существует ");

            return true;
        }

        static public ushort InputMenuOption()
        {
            bool inputProccess = true;
            ushort option = 0;

            while (inputProccess)
            {
                Console.Write("Введите опцию меню: ");
                string? input = Console.ReadLine();

                if (ushort.TryParse(input, out option))
                {
                    if (0 <= option && option <= MENU_OPTIONS_QUANTITY)
                    {
                        inputProccess = false;
                    }
                    else
                    {
                        Console.WriteLine($"[ Menu ] Введенная опция должна быть в диапазоне от 0 до {MENU_OPTIONS_QUANTITY}");
                    }
                }
                else
                {
                    Console.WriteLine("[ Menu ] Введите целое неотрицательное число");
                }
            }

            return option;
        }
    }   
}