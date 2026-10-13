using System;
using System.ComponentModel.Design;
using System.Drawing;
using Lab06.MenuModule;
using Lab06.ConsoleInputModule;
using Lab06.NpgsqlDataBaseModule;
using Lab06.AppControllerModule;

namespace Lab06.MainModule
{
    public class Program
    {
        static public void Main()
        {
            AppController controller = new();

            while (controller.IsRunning)
            {
                Menu.PrintMenu();

                Console.Write("Введите пункт меню: ");
                uint menuOpt = ConsoleInput.GetUintRange(0, Menu.MENU_OPTIONS_QUANTITY);
                
                controller.ExecuteMenuOption(menuOpt);
            }
        }
    }  
}