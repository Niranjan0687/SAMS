using System.ComponentModel.DataAnnotations;

namespace SmsAPI.Models
{
    public class Course
    {
        public int id { get; set; }
        [Required]
        public string Title { get; set; }
        public int Credits{ get; set; }
        public ICollection<EnrolledCourses> EnrolledCourses { get; set; }

    }
}
