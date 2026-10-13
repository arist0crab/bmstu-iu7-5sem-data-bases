using System;
using System.ComponentModel.Design;
using System.Drawing;
using Lab06.MenuModule;
using Lab06.NpgsqlDataBaseModule;

namespace Lab06.MainModule
{
    public class Program
    {
        private static NpgsqlDataBase? _dataBase = null;
        private static readonly Dictionary<int, Action> _menuActions = new()
        {
            { 1, () => _dataBase?.Task01() },
            { 2, () => _dataBase?.Task02() }
        };

        static public void Main()
        {
            Menu.Initialize(_menuActions);
            SetUpDataBase();

            bool mainCycle = true;
            while (mainCycle)
            {
                Menu.PrintMenu();
                ushort opt = Menu.InputMenuOption();
                mainCycle = Menu.ExecuteMenuOption(opt);
            }
        }

        static private void SetUpDataBase()
        {
            if (_dataBase is null)
            {
                _dataBase = new NpgsqlDataBase("localhost", "postgres", "postgres");
                _dataBase.CreateDataSource();
            }
        } 
    }  
}