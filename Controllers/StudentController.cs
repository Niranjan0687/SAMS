using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmsAPI.Data;
using SmsAPI.Models;
using System.ComponentModel.Design.Serialization;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace SmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly AdmissionDbContext _context;
        public StudentController(AdmissionDbContext context) 
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult getStudentList()
        {
            var students = _context.Students.ToList();
            return Ok(students);
        }
        [HttpGet("{id}")]
        public IActionResult getStudent(int id)
        {
            var student = _context.Students.Find(id);
            if(student == null)
            {
                return NotFound("Student not found");
            }
            return Ok(student);
        }
        [HttpPost]
        public async Task<IActionResult> CreateStudent(Student objstd)
        {
            _context.Students.Add(objstd);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(getStudent), new { id = objstd.Id }, objstd);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id,Student std)
        {
            var student = await _context.Students.FindAsync(id);
            if(student==null)return NotFound();
            student.FullName = std.FullName;
            student.Course = std.Course;
            student.AdmissionDate = std.AdmissionDate;
            student.DateOfBirth = std.DateOfBirth;
            student.Email = std.Email;

            await _context.SaveChangesAsync();
            return NoContent();

        }
        [HttpDelete]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost("with-courses")]
        public async Task<IActionResult> AddStudentwithCourse(StudentWithCoursesDto objstd)
        {
            var student = new Student
            {
                Id = objstd.Id,
                FullName = objstd.FullName,
                Email = objstd.Email,
                DateOfBirth = objstd.DateOfBirth,
                AdmissionDate = objstd.AdmissionDate,
                EnrolledCourses = new List<EnrolledCourses>()
            };
            var courses=await _context.Course.Where(c=>objstd.EnrolledCourseIds.Contains(c.id)).ToListAsync();
            foreach(var course in courses)
            {
                student.EnrolledCourses.Add(new EnrolledCourses
                {
                    CourseId = course.id,
                    student = student
                });
            }
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
          return Ok(new StudentWithCoursesDto
            {
                Id = student.Id,
                FullName = student.FullName,
                Email = student.Email,
                DateOfBirth = student.DateOfBirth,
                AdmissionDate = student.AdmissionDate,
                EnrolledCourseIds = student.EnrolledCourses.Select(ec => ec.CourseId).ToList()
            });

        }
        [HttpGet("studentlist")]
        public async Task<IActionResult> getCourseandStudent()
        {
            var stdDetails =await _context.StudentCourseDetailsDto.FromSqlRaw("Exec GetStudentCourseDetails").ToListAsync();
           
            return Ok(JsonSerializer.Serialize(stdDetails));
        }

        [HttpGet("{studentid}/stdwith_course")]
        public async Task<IActionResult> getStudentwiseCourseDetails(int studentid)
        {
             var student = await _context.Students.Where(s => s.Id == studentid)
            .Include(s => s.EnrolledCourses)
            .ThenInclude(ec => ec.course)
            .Select(s => new StudentCourseInformation
            {
                StudentId = s.Id,
                FullName = s.FullName,
                DateOfBirth = s.DateOfBirth,
                Courses = s.EnrolledCourses.Select(sc => new Course
                {
                    id = sc.course.id,
                    Title = sc.course.Title,
                    Credits = sc.course.Credits
                }).ToList()
            }).FirstOrDefaultAsync();

                    if (student == null)
                        return NotFound("Student Not Found!");

                    var jsonresult = JsonSerializer.Serialize(student);
                    return Ok(jsonresult);
        }
        [HttpGet("Allstdwith_course")]
        public async Task<IActionResult> getAllStudentwiseCourseDetails()
        {
            var student = await _context.Students         
           .Select(s => new StudentCourseInformation
           {
               StudentId = s.Id,
               FullName = s.FullName,
               DateOfBirth = s.DateOfBirth,
               Courses = s.EnrolledCourses.Select(sc => new Course
               {
                   id = sc.course.id,
                   Title = sc.course.Title,
                   Credits = sc.course.Credits
               }).ToList()
           }).ToListAsync();

            if (student == null)
                return NotFound("Student Not Found!");

            var jsonresult = JsonSerializer.Serialize(student);
            return Ok(jsonresult);
        }

    }
}
