using System;
using Npgsql;

namespace Lab06.NpgsqlDataBaseModule
{
    public class NpgsqlDataBase(
        string hostName, 
        string userName,
        string databaseName
    )
    {
        private readonly string _hostName = hostName;
        private readonly string _userName = userName;
        private readonly string _databaseName = databaseName;
        private readonly string _password = "88005553535";
        private bool _createdDataSource = false;

        private NpgsqlDataSource? _dataSource = null;

        public string HostName => _hostName;
        public string UserName => _userName;
        public string DatabaseName => _databaseName;
        public bool IsActive => _createdDataSource;

        public void  CreateDataSource()
        {
            var connectionString = $"Host={_hostName};Username={_userName};Password={_password};Database={_databaseName}";
            _dataSource = NpgsqlDataSource.Create(connectionString);
            _createdDataSource = true;
        }

        // 1. Посчитать количество активных учеников в школе
        public void Task01()
        {
            if (_dataSource is not null)
            {
                using var command = _dataSource.CreateCommand("SELECT COUNT(*) FROM students WHERE is_active = true");
                using var reader = command.ExecuteReader();

                while (reader.Read())
                    Console.WriteLine($"Количество активных учеников в школе: {reader.GetInt32(0)}");
            }
        }

        // 2. Найти всех учеников с активными справками
        public void Task02()
        {
            if (_dataSource is not null)
            {
                string query = 
                """
                    SELECT
                        S.id
                        , S.first_name
                        , S.last_name
                        , MC.id AS mc_id
                    FROM students AS S
                    LEFT JOIN medical_certificates AS MC ON S.id = MC.student_id
                    WHERE MC.id IS NULL
                """;
                using var command = _dataSource.CreateCommand(query);
                using var reader = command.ExecuteReader();

                int i = 1;
                while (reader.Read())
                    Console.WriteLine($"{++i}. {reader.GetString(1)} {reader.GetString(2)}");
            }
        }
    }
}