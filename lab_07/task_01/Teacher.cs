using System;
using System.Reflection.PortableExecutable;
using Faker;


namespace Lab07.Task01.TeacherModule
{
    public class Teacher(
        int id, 
        string firstName, 
        string lastname, 
        DateTime birthDate, 
        int? mentorTeacherId
    )
    {
        public int Id { get; set; } = id;
        public string Firstname { get; set; } = firstName;
        public string LastName { get; set; } = lastname;
        public DateTime BirthDate { get; set; } = birthDate;
        public int? MentorTeacherId { get; set; } = mentorTeacherId;

        public static Teacher Generate(int id)
        {
            Random rand = new();
            
            string firstName = Faker.NameFaker.FirstName();
            string lastname = Faker.NameFaker.LastName();
            DateTime birthDate = Faker.DateTimeFaker.BirthDay();
            int? mentorId = rand.Next(0, 10) < 4 ? rand.Next(1, id) : (int?)null;

            return new Teacher(id, firstName, lastname, birthDate, mentorId);
        }

        public static void PrintTeachers(List<Teacher> teachers)
        {
            foreach (Teacher t in teachers)
                t.Print();
        }

        public void Print()
        {
            Console.WriteLine($"{Firstname} {LastName} {BirthDate} {MentorTeacherId}");
        }
    }
}