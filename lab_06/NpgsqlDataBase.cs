using System;
using Npgsql;

namespace Lab06.NpgsqlDataBaseModule
{
    public class NpgsqlDataBase
    {
        private readonly string _hostName;
        private readonly string _userName;
        private readonly string _databaseName;
        private readonly string _password = "88005553535";

        private readonly NpgsqlDataSource _dataSource;

        public string HostName => _hostName;
        public string UserName => _userName;
        public string DatabaseName => _databaseName;

        public NpgsqlDataBase(string hostName, string userName, string databaseName)
        {
            _hostName = hostName;
            _userName = userName;
            _databaseName = databaseName;

            var connectionString = $"Host={_hostName};Username={_userName};Password={_password};Database={_databaseName}";
            _dataSource = NpgsqlDataSource.Create(connectionString);
        }

        // 1. Посчитать количество активных учеников в школе
        public void Task01()
        {
            using var command = _dataSource.CreateCommand("SELECT COUNT(*) FROM students WHERE is_active = true");
            using var reader = command.ExecuteReader();

            while (reader.Read())
                Console.WriteLine($"Количество активных учеников в школе: {reader.GetInt32(0)}");
        }

        // 2. Найти всех учеников с активными справками
        public void Task02()
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

        // 3. Найти самого старшего ученика класса
        public void Task03(int classId)
        {
            string query = 
            """
                WITH AgeRankedStudents AS (
                    SELECT 
                        first_name,
                        last_name,
                        class_group_id,
                        ROW_NUMBER() OVER (PARTITION BY class_group_id ORDER BY birth_date ASC) AS age_rank
                    FROM students
                )
                SELECT *
                FROM AgeRankedStudents
                WHERE age_rank = 1 AND class_group_id = $1
            """;

            using var command = _dataSource.CreateCommand(query);
            command.Parameters.AddWithValue(classId);
            using var reader = command.ExecuteReader();

            while (reader.Read())
                Console.WriteLine($"Самый старший ученик в классе id = {classId}: {reader.GetString(0)} {reader.GetString(1)}");
        }

        // 4. Вывести список всех таблиц базы данных
        public void Task04()
        {
            string query = 
            """
                SELECT
                    table_name
                FROM information_schema.tables
                WHERE table_schema NOT IN ('pg_catalog', 'information_schema')
            """;

            using var command = _dataSource.CreateCommand(query);
            using var reader = command.ExecuteReader();

            while (reader.Read())
                Console.WriteLine($"{reader.GetString(0)}");
        }

        // 5. Вывести количество учеников в классе 
        public void Task05(int classGroupId)
        {
            string query = "SELECT public.calc_students_in_class($1)";
            using var command = _dataSource.CreateCommand(query);
            command.Parameters.AddWithValue(classGroupId);
            using var reader = command.ExecuteReader();

            while (reader.Read())
                Console.WriteLine($"Количество учеников в классе с id = {classGroupId}: {reader.GetInt32(0)}");
        }

        // 6. Вывести таблицу всех медицинских сертификатов студента
        public void Task06(int studentId)
        {
            string query = "SELECT * FROM get_students_medical_certificates($1)";
            using var command = _dataSource.CreateCommand(query);
            command.Parameters.AddWithValue(studentId);
            using var reader = command.ExecuteReader();

            while (reader.Read())
                Console.WriteLine($"id сертификата: {reader.GetInt32(0)}, {reader.GetString(1)}");
        }

        // 7. Поменять классного руководителя у класса
        public void Task07(int pClassId, int pTeacherId)
        {
            string query = "CALL change_class_teacher($1, $2)";
            using var command = _dataSource.CreateCommand(query);
            command.Parameters.AddWithValue(pClassId);
            command.Parameters.AddWithValue(pTeacherId);
            using var reader = command.ExecuteReader();

            while (reader.Read())
                Console.WriteLine($"Классный руководитель класса id = {pClassId} был сменен на учителя id = {pTeacherId}: ");
        }

        // 8. Получить текущую версию сервера
        public void Task08()
        {
            string query = "SELECT version();";
            using var command = _dataSource.CreateCommand(query);
            using var reader = command.ExecuteReader();

            while (reader.Read())
                Console.WriteLine($"{reader.GetString(0)}");
        }
    }
}