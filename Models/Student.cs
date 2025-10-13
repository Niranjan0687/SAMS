using System.ComponentModel.DataAnnotations;

namespace SmsAPI.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Course { get; set; }

        public DateTime AdmissionDate { get; set; }
        public ICollection<EnrolledCourses> EnrolledCourses { get; set; }

    }
}
