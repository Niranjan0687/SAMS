using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmsAPI.Data;
using SmsAPI.Models;


namespace SmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly AdmissionDbContext _context;
        public EnrollmentController(AdmissionDbContext context)
        {
               _context = context;
        }
        // Implement enrollment-related actions here
        [HttpPost("{studentId}/enroll")]
        public  async Task<IActionResult> Enroll(int studentId,[FromBody] List<int> courseIds)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student == null) return NotFound("Student not found");
           foreach(var CourseId in courseIds)
            {
                var isExits =await _context.EnrolledCourses.AnyAsync(sc => sc.StudentId == studentId && sc.CourseId== CourseId);

                if (!isExits)
                {
                 _context.EnrolledCourses.Add(new EnrolledCourses
                    {
                         CourseId = CourseId,
                            StudentId = studentId

                 });
                }             
            }
            await _context.SaveChangesAsync();
            return Ok("Student enrolled in courses successfully");
        }
    }
}
