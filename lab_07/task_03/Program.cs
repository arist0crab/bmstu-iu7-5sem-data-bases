using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Lab07.Task03.AppDbContextModule;
using Lab07.Task03.StudentModule;

namespace Lab07.Task03
{
    class Program
    {
        static void Main(string[] args)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            using var db = new AppDbContext();

            Console.WriteLine("1. Выборка всех активных студентов:");
            var activeStudents = db.Students
                .Where(s => s.IsActive)
                .OrderBy(s => s.LastName)
                .ToList();

            foreach (var student in activeStudents)
                Console.WriteLine($"- {student.LastName} {student.FirstName}");
            Console.WriteLine();

            Console.WriteLine("2. Студенты вместе с их классами:");
            var studentsWithGroups = db.Students
                .Include(s => s.StudentClassGroup)
                .ToList();

            foreach (var s in studentsWithGroups)
            {
                string groupName = s.StudentClassGroup != null ? $"{s.StudentClassGroup.Grade}-{s.StudentClassGroup.LetterId}" : "Без класса";
                Console.WriteLine($"- {s.LastName} {s.FirstName} (Класс: {groupName})");
            }
            Console.WriteLine();

            Console.WriteLine("3. Добавление, изменение и удаления:");

            var newStudent = new Student
            {
                FirstName = "Тест",
                LastName = "Тестов",
                Sex = 'M',
                BirthDate = new DateTime(2010, 1, 1),
                ClassGroupId = 1,
                IsActive = true
            };

            db.Students.Add(newStudent);
            db.SaveChanges();

            Console.WriteLine("Студент Тестов был добавлен");

            var studentToUpdate = db.Students.FirstOrDefault(s => s.LastName == "Тестов");
            if (studentToUpdate != null)
            {
                studentToUpdate.LastName = "ИзмененныйТестов";
                db.SaveChanges();
            }

            Console.WriteLine("Студент тестов был изменен на ИзмененныйТестов");

            var studentToDelete = db.Students.FirstOrDefault(s => s.LastName == "ИзмененныйТестов");
            if (studentToDelete != null)
            {
                db.Students.Remove(studentToDelete);
                db.SaveChanges();
            }

            Console.WriteLine("Студент ИзмененныйТестов был удален\n");

            Console.WriteLine("4. Вызов хранимой процедуры из базы данных:");
            try
            {
                int classId = 52;
                int newTeacherId = 52;

                db.Database.ExecuteSqlRaw("CALL change_class_teacher({0}, {1})", classId, newTeacherId);
                Console.WriteLine("Хранимая процедура была вызвана");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ Ошибка при вызове процедуры ]: {ex.Message}");
            }
        }
    }
}