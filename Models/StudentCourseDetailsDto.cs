namespace SmsAPI.Models
{
    public class StudentCourseDetailsDto
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }

    }
}
