using Lab07.Task01.TeacherModule;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Principal;

namespace Lab07.Task01.Program
{
    public class Program
    {
        public static void Main()
        {
            List<Teacher> teachers = [];

            for (int i = 1; i < 100; ++i)
                teachers.Add(Teacher.Generate(i));

            Console.WriteLine("\n=== Учителя старше 1980 года ===\n");

            var older1980 = (from t in teachers where t.BirthDate < new DateTime(1980, 1, 1) orderby t.LastName select t).ToList();
            Teacher.PrintTeachers(older1980);

            Console.WriteLine("\n=== Учителя с короткими именами ===\n");

            var shorterThan10LettersNames = (from t in teachers let fullName = string.Join(" ", t.Firstname, t.LastName) where fullName.Length <= 10 select t).ToList();
            Teacher.PrintTeachers(shorterThan10LettersNames);

            Console.WriteLine("\n=== Группировка по году рождения ===\n");

            var groupByBirthDate = (from t in teachers group t by t.BirthDate into gr select new { BirthDate = gr.Key, Quantity = gr.Count(), Teachers = gr }).ToList();
            foreach (var gr in groupByBirthDate)
            {
                Console.Write($"{gr.BirthDate} ({gr.Quantity}): ");
                foreach (var t in gr.Teachers)
                    Console.Write($"{t.Firstname} {t.LastName}; ");
                Console.WriteLine();
            }

            Console.WriteLine("\n=== Учителя и их наставники ===\n");

            var mentorTeacherpairs = (from t in teachers join m in teachers on t.MentorTeacherId equals m.Id select new { TeacherName = t.LastName, MentorName = m.LastName }).ToList();
            foreach (var mt in mentorTeacherpairs)
                Console.WriteLine($"Teacher = {mt.TeacherName}, Mentor = {mt.MentorName}");

            Console.WriteLine("\n=== Учителя с упрощенными данными ===\n");

            var simpleTeachers = (from t in teachers select new { FirstName = t.Firstname, LastName = t.LastName });
            foreach (var t in simpleTeachers)
                Console.WriteLine($"{t.FirstName} {t.LastName}");
        }
    }
}
