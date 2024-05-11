using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using TrainingCenter.DTO;
using TrainingCenter.interfaces;
using TrainingCenter.Models;

namespace TrainingCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICoursesRepository _coursesRepository;
        private readonly IMapper _mapper;

        public CoursesController(ICoursesRepository coursesRepository, IMapper mapper)
        {
            _coursesRepository = coursesRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCourses()
        {
            var courses = _coursesRepository.GetCourses();
            if (courses == null || !courses.Any())
                return NotFound("No courses found.");
            var coursesDto = _mapper.Map<List<CourseDTO>>(courses);
            return Ok(coursesDto);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCourse(int id)
        {
            var course = _coursesRepository.GetCourse(id);
            if (course == null)
                return NotFound("Course not found.");
            var courseDto = _mapper.Map<CourseDTO>(course);
            return Ok(courseDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddCourse([FromBody] CourseDTO courseDto)
        {
            // validation
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (_coursesRepository.GetCourses().Any(c => c.Name == courseDto.Name))
                return BadRequest("Course already exists.");

            if (courseDto.StartDate >= courseDto.EndDate)
                return BadRequest("Start date must be before end date.");

            // processing

            var course = _mapper.Map<Course>(courseDto);
            _coursesRepository.AddCourse(course);



            // response
            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateCourse(int id, [FromBody] CourseDTO courseDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var course = _coursesRepository.GetCourse(id);
            if (course == null)
                return NotFound("Course not found.");
            _mapper.Map(courseDto, course);
            _coursesRepository.UpdateCourse(course);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteCourse(int id)
        {
            var course = _coursesRepository.GetCourse(id);
            if (course == null)
                return NotFound("Course not found.");
            _coursesRepository.DeleteCourse(id);
            return NoContent();
        }
    }
}
