namespace SmsAPI.Models
{
    public class StudentWithCoursesDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Course { get; set; }
        public DateTime AdmissionDate { get; set; }
        public List<int> EnrolledCourseIds { get; set; }
    }
}
