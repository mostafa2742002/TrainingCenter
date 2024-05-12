using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using TrainingCenter.DTO;
using TrainingCenter.interfaces;
using TrainingCenter.Models;
using Microsoft.AspNetCore.Authorization;
namespace TrainingCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        private readonly ICoursesRepository _coursesRepository;

        public StudentsController(IStudentRepository studentRepository, IMapper mapper, ICoursesRepository coursesRepository)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _coursesRepository = coursesRepository;
        }
        // GetStudents, GetStudent, AddStudent, UpdateStudent, DeleteStudent, GetStudentCourses, GetStudentCourse, AddStudentCourse, UpdateStudentCourse, DeleteStudentCourse

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<StudentDTO>))]
        public IActionResult GetStudents()
        {
            var students = _studentRepository.GetStudents();
            if (students == null || !students.Any())
                return NotFound("No students found.");
            var studentDtos = _mapper.Map<List<StudentDTO>>(students);
            return Ok(studentDtos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(StudentDTO))]
        [ProducesResponseType(404)]
        public IActionResult GetStudent(int id)
        {
            var student = _studentRepository.GetStudent(id);
            if (student == null)
                return NotFound("Student not found.");
            var studentDto = _mapper.Map<StudentDTO>(student);
            return Ok(studentDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(StudentDTO))]
        [ProducesResponseType(400)]
        public IActionResult AddStudent([FromBody] StudentDTO studentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var student = _mapper.Map<Student>(studentDto);

            _studentRepository.AddStudent(student);
            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, studentDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateStudent(int id, [FromBody] StudentDTO studentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var student = _studentRepository.GetStudent(id);
            if (student == null)
                return NotFound("Student not found.");
            _mapper.Map(studentDto, student);
            _studentRepository.UpdateStudent(student);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteStudent(int id)
        {
            var student = _studentRepository.GetStudent(id);
            if (student == null)
                return NotFound("Student not found.");
            _studentRepository.DeleteStudent(id);
            return NoContent();
        }

        [HttpGet("{studentId}/courses")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<StudentCourseDTO>))]
        public IActionResult GetStudentCourses(int studentId)
        {
            List<StudentCourse> studentcourses = _studentRepository.GetStudentCourses(studentId);
            if (studentcourses  == null || !studentcourses.Any())
                return Ok(new List<StudentCourse>());

            var studentcourseDtos = _mapper.Map<List<StudentCourseDTO>>(studentcourses);

            return Ok(studentcourseDtos);
        }

        [HttpGet("{studentId}/courses/{courseId}")]
        [ProducesResponseType(200, Type = typeof(StudentCourseDTO))]
        public IActionResult GetStudentCourse(int studentId, int courseId)
        {
            var course = _studentRepository.GetStudentCourse(studentId, courseId);
            if (course == null)
                return Ok(null);
            var courseDto = _mapper.Map<CourseDTO>(course);
            return Ok(courseDto);
        }

        [HttpPost("{studentId}/courses")]
        [ProducesResponseType(201, Type = typeof(StudentCourseDTO))]
        public IActionResult AddStudentCourse(int studentId,int courseId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var course = _coursesRepository.GetCourse(courseId);
            var student = _studentRepository.GetStudent(studentId);
            if (course == null)
                return NotFound("Course not found.");
            if (student == null)
                return NotFound("Student not found.");
    
            
            _studentRepository.AddStudentCourse(
                new StudentCourse
                {
                    StudentId = studentId,
                    CourseId = courseId,
                    Course = course,
                    Student = student,
                    RegistrationDate = DateTime.Now,
                    Grade = 0,
                    Status = "Active"
                }
            );

            return CreatedAtAction(nameof(GetStudentCourse), new { studentId = studentId, courseId = courseId }, new StudentCourseDTO());
        }

        [HttpPut("{studentId}/courses/{courseId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateStudentCourse(int studentId, int courseId, [FromBody] CourseDTO courseDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var course = _studentRepository.GetStudentCourse(studentId, courseId);
            if (course == null)
                return NotFound("Course not found.");
            _mapper.Map(courseDto, course);
            _studentRepository.UpdateStudentCourse(course);
            return NoContent();
        }

        [HttpDelete("{studentId}/courses/{courseId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteStudentCourse(int studentId, int courseId)
        {
            var course = _studentRepository.GetStudentCourse(studentId, courseId);
            if (course == null)
                return NotFound("Course not found.");
            _studentRepository.DeleteStudentCourse(studentId, courseId);
            return NoContent();
        }

    }
}
