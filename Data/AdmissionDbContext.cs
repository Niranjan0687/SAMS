using Microsoft.EntityFrameworkCore;
using SmsAPI.DTOs;
using SmsAPI.Models;
using System.Security.Cryptography.X509Certificates;

namespace SmsAPI.Data
{
    public class AdmissionDbContext : DbContext
    {
        public AdmissionDbContext(DbContextOptions<AdmissionDbContext> options) :base(options) { }
        public DbSet<Student> Students { get; set; }
        public DbSet<EnrolledCourses> EnrolledCourses { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<StudentCourseDetailsDto> StudentCourseDetailsDto { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<PersonPhone> PersonPhones { get; set; }
        public DbSet<Person> person { get; set; }
        public DbSet<PersonPhoneDetailsDto> personPhoneDetailsDtos { get; set; }

       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentCourseDetailsDto>().HasNoKey();
            modelBuilder.Entity<PersonPhoneDetailsDto>().HasNoKey();
            modelBuilder.Entity<PersonPhone>().HasNoKey();
        }
    }
   
}
