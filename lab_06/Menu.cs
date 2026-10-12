using System;
using System.Drawing;

namespace Lab06.MenuModule
{
    public class Menu
    {
        const ushort MENU_OPTIONS_QUANTITY = 10;

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
                        Console.WriteLine($"[ ERROR ] Введенная опция должна быть в диапазоне от 0 до {MENU_OPTIONS_QUANTITY}");
                    }
                }
                else
                {
                    Console.WriteLine("[ ERROR ] Введите целое неотрицательное число");
                }
            }

            return option;
        }
    }   
}