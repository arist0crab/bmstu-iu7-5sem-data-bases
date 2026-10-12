using System;
using System.ComponentModel.Design;
using System.Drawing;
using Lab06.MenuModule;

namespace Lab06.MainModule
{
    public class Program
    {
        static public void Main()
        {
            bool mainCycle = true;
            while (mainCycle)
            {
                Menu.PrintMenu();
                ushort opt = Menu.InputMenuOption();
                Console.WriteLine($"Введенное число: {opt}");

                if (opt == 0)
                    mainCycle = false;
            }
        }
    }   
}