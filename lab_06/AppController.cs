using System;
using Lab06.ConsoleInputModule;
using Lab06.MenuModule;
using Lab06.NpgsqlDataBaseModule;

namespace Lab06.AppControllerModule
{
    public class AppController
    {
        private readonly NpgsqlDataBase _dataBase = new("localhost", "postgres", "postgres");
        private readonly Dictionary<uint, Action>? _menuActions =  null;
        private bool _isRunning = true;

        public bool IsRunning => _isRunning;

        public AppController()
        {
            _menuActions = new()
            {
                { 0, ExitProgram },
                { 1, Task01 },
                { 2, Task02 },
                { 3, Task03 },
                { 4, Task04 },
                { 5, Task05 },
                { 6, Task06 },
                { 7, Task07 },
                { 8, Task08 },
                { 9, Task09 },
                { 10, Task10 }
            };
        }

        public void ExecuteMenuOption(uint menuOption)
        {
            if (_dataBase is null)
            {
                Console.WriteLine("[ AppController ] Невозможно выполнить запрос: базаданных не инициализирована");
                return;
            }

            if (_menuActions is null)
            {
                Console.WriteLine("[ AppController ] Невозможно выполнить запрос: меню действий не инициализировано");
                return;
            }

            if (_menuActions.TryGetValue(menuOption, out var action))
            {
                action.Invoke();
            }
            else
            {
                Console.Write("[ AppController ] Невозможно выполнить запрос: данной опции меню не сущствует");
            }
        }

        private void ExitProgram()
        {
            _isRunning = false;
        }

        private void Task01()
        {
            _dataBase.Task01();
        }
        
        private void Task02()
        {
            _dataBase.Task02();
        }

        private void Task03()
        {
            uint minClassGroupId = 1, maxClassGroupId = 500;

            Console.Write($"Введите id класса от {minClassGroupId} до {maxClassGroupId}: ");
            uint classGroupId = ConsoleInput.GetUintRange(1, 500);

            _dataBase.Task03((int)classGroupId);
        }

        private void Task04()
        {
            _dataBase.Task04();
        }

        private void Task05()
        {
            
        }

        private void Task06()
        {
            
        }

        private void Task07()
        {
            
        }

        private void Task08()
        {
            
        }

        private void Task09()
        {
            
        }

        private void Task10()
        {
            
        }
    }
}