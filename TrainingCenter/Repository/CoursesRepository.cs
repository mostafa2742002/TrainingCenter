using Microsoft.EntityFrameworkCore;
using TrainingCenter.Data;
using TrainingCenter.Models;
using TrainingCenter.DTO;
using AutoMapper;
using TrainingCenter.interfaces;

namespace TrainingCenter.Repository
{
    public class CoursesRepository : ICoursesRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CoursesRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<Course> GetCourses()
        {
            return _context.Courses.ToList();
        }

        public Course GetCourse(int id)
        {
            return _context.Courses.Find(id);
        }

        public void AddCourse(Course course)
        {
            _context.Courses.Add(course);
            _context.SaveChanges();
        }

        public void UpdateCourse(Course course)
        {
            _context.Courses.Update(course);
            _context.SaveChanges();
        }

        public void DeleteCourse(int id)
        {
            var course = _context.Courses.Find(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                _context.SaveChanges();
            }
        }

        public List<StudentCourse> GetCourseStudents(int courseId)
        {
            return _context.StudentCourses.Where(sc => sc.CourseId == courseId).ToList();
        }

        public StudentCourse GetCourseStudent(int courseId, int studentId)
        {
            return _context.StudentCourses.FirstOrDefault(sc => sc.CourseId == courseId && sc.StudentId == studentId);
        }

        public void AddCourseStudent(StudentCourse studentCourse)
        {
            _context.StudentCourses.Add(studentCourse);
            _context.SaveChanges();
        }

        public void UpdateCourseStudent(StudentCourse studentCourse)
        {
            _context.StudentCourses.Update(studentCourse);
            _context.SaveChanges();
        }

        public void DeleteCourseStudent(int courseId, int studentId)
        {
            var studentCourse = _context.StudentCourses.FirstOrDefault(sc => sc.CourseId == courseId && sc.StudentId == studentId);
            if (studentCourse != null)
            {
                _context.StudentCourses.Remove(studentCourse);
                _context.SaveChanges();
            }
        }
    }
}
