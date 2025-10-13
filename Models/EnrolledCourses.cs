using Microsoft.EntityFrameworkCore;

namespace SmsAPI.Models
{
    [PrimaryKey(nameof(StudentId), nameof(CourseId))]
    public class EnrolledCourses
    {

        public int StudentId { get; set; }
        public virtual Student student { get; set; }
        public int CourseId { get; set; }
        public virtual Course course { get; set; }
    }
}
