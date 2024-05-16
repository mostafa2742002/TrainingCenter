using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TrainingCenter.DTO;
using TrainingCenter.interfaces;
using TrainingCenter.Models;
using Microsoft.AspNetCore.Authorization;
using TrainingCenter.interfaces;

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
            
            // check if the email already exists
            if (_studentRepository.GetStudents().Any(s => s.Email == studentDto.Email))
                return BadRequest("Email already exists.");


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
            List<StudentCourse> studentCourses = _studentRepository.GetStudentCourses(studentId);
            if (studentCourses == null || !studentCourses.Any())
                return Ok(new List<StudentCourse>());

            var studentCourseDtos = _mapper.Map<List<StudentCourseDTO>>(studentCourses);
            return Ok(studentCourseDtos);
        }

        [HttpGet("{studentId}/courses/{courseId}")]
        [ProducesResponseType(200, Type = typeof(StudentCourseDTO))]
        public IActionResult GetStudentCourse(int studentId, int courseId)
        {
            var course = _studentRepository.GetStudentCourse(studentId, courseId);
            if (course == null)
                return Ok(null);
            var courseDto = _mapper.Map<StudentCourseDTO>(course);
            return Ok(courseDto);
        }

        [HttpPost("{studentId}/courses")]
        [ProducesResponseType(201, Type = typeof(StudentCourseDTO))]
        public IActionResult AddStudentCourse(int studentId, [FromBody] StudentCourseDTO studentCourseDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var course = _coursesRepository.GetCourse(studentCourseDto.CourseId);
            var student = _studentRepository.GetStudent(studentId);
            if (course == null)
                return NotFound("Course not found.");
            if (student == null)
                return NotFound("Student not found.");

            var studentCourse = new StudentCourse
            {
                StudentId = studentId,
                CourseId = studentCourseDto.CourseId,
                Course = course,
                Student = student,
                RegistrationDate = DateTime.Now,
                Grade = 0,
                Status = "Active"
            };

            _studentRepository.AddStudentCourse(studentCourse);

            return CreatedAtAction(nameof(GetStudentCourse), new { studentId = studentId, courseId = studentCourse.CourseId }, studentCourseDto);
        }

        [HttpPut("{studentId}/courses/{courseId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateStudentCourse(int studentId, int courseId, [FromBody] StudentCourseDTO studentCourseDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var studentCourse = _studentRepository.GetStudentCourse(studentId, courseId);
            if (studentCourse == null)
                return NotFound("Student course not found.");
            _mapper.Map(studentCourseDto, studentCourse);
            _studentRepository.UpdateStudentCourse(studentCourse);
            return NoContent();
        }

        [HttpDelete("{studentId}/courses/{courseId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteStudentCourse(int studentId, int courseId)
        {
            var studentCourse = _studentRepository.GetStudentCourse(studentId, courseId);
            if (studentCourse == null)
                return NotFound("Student course not found.");
            _studentRepository.DeleteStudentCourse(studentId, courseId);
            return NoContent();
        }
    }
}
