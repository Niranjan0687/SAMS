namespace SmsAPI.Models
{
    public class StudentCourseInformation
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<Course> Courses { get; set; }

    }
}
