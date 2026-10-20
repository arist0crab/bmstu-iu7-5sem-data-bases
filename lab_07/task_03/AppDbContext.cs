using System;
using Microsoft.EntityFrameworkCore;
using Lab07.Task03.AbsentModule;
using Lab07.Task03.CabinetModule;
using Lab07.Task03.ClassGroupModule;
using Lab07.Task03.DeskModule;
using Lab07.Task03.MedicalCertificatesModule;
using Lab07.Task03.SeatModule;
using Lab07.Task03.SeatingsAssignmentsModule;
using Lab07.Task03.StudentModule;
using Lab07.Task03.TeacherModule;

namespace Lab07.Task03.AppDbContextModule
{
    public class AppDbContext : DbContext
    {
        public DbSet<Absent> Absents { get; set; }
        public DbSet<Cabinet> Cabinets { get; set; }
        public DbSet<ClassGroup> ClassGroups { get; set; }
        public DbSet<Desk> Desks { get; set; }
        public DbSet<MedicalCertificates> MedicalCertificates { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<SeatingsAssignments> SeatingsAssignments { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=postgres;Username=postgres;Password=88005553535");
        }
    }
}