using Microsoft.EntityFrameworkCore;
using TrainingCenter.Data;
using TrainingCenter.interfaces;
using TrainingCenter.Models;

namespace TrainingCenter.Repository
{
    public class CoursesRepository : ICoursesRepository
    {
        private readonly DataContext _context;

        public CoursesRepository(DataContext context)
        {
            _context = context;
        }

        public List<Course> GetCourses()
        {
            return [.. _context.Set<Course>()];
        }

        public Course GetCourse(int id)
        {
            return _context.Set<Course>().Find(id);
        }

        public void AddCourse(Course course)
        {
            _context.Set<Course>().Add(course);
            _context.SaveChanges();
        }

        public void UpdateCourse(Course course)
        {
            _context.Set<Course>().Update(course);
            _context.SaveChanges();
        }

        public void DeleteCourse(int id)
        {
            var course = _context.Set<Course>().Find(id)
                ?? throw new Exception("Course not found");
            
            _context.Set<Course>().Remove(course);
            _context.SaveChanges();
            
        }

        public List<StudentCourse> GetCourseStudents(int courseId)
        {
            return _context.Set<StudentCourse>().Where(sc => sc.CourseId == courseId).ToList();
        }

        public StudentCourse GetCourseStudent(int courseId, int studentId)
        {
            return _context.Set<StudentCourse>()
                .FirstOrDefault(sc => sc.CourseId == courseId && sc.StudentId == studentId)
                ?? throw new Exception("Student course not found");
        }

        public void AddCourseStudent(StudentCourse studentCourse)
        {
            _context.Set<StudentCourse>().Add(studentCourse);
            _context.SaveChanges();
        }

        public void UpdateCourseStudent(StudentCourse studentCourse)
        {
            _context.Set<StudentCourse>().Update(studentCourse);
            _context.SaveChanges();
        }

        public void DeleteCourseStudent(int courseId, int studentId)
        {
            var studentCourse = _context.Set<StudentCourse>()
                .FirstOrDefault(sc => sc.CourseId == courseId && sc.StudentId == studentId)
                ?? throw new Exception("Student course not found");

            _context.Set<StudentCourse>().Remove(studentCourse);
            _context.SaveChanges();
            
        }
    }
}
