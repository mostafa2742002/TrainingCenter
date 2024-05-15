using TrainingCenter.Data;
using TrainingCenter.interfaces;
using TrainingCenter.Models;
using Microsoft.EntityFrameworkCore;
namespace TrainingCenter.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly DataContext _context;

        public StudentRepository(DataContext context)
        {
            _context = context;
        }

        public List<Student> GetStudents()
        {
            return _context.Students.ToList();
        }

        public Student GetStudent(int id)
        {
            return _context.Students.Find(id);
        }

        public void AddStudent(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
        }

        public void UpdateStudent(Student student)
        {
            _context.Students.Update(student);
            _context.SaveChanges();
        }

        public void DeleteStudent(int id)
        {
            var student = _context.Students.Find(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
            }
        }

        public List<StudentCourse> GetStudentCourses(int studentId)
        {
            return _context.StudentCourses.Where(sc => sc.StudentId == studentId).ToList();
        }

        public StudentCourse GetStudentCourse(int studentId, int courseId)
        {
            return _context.StudentCourses.FirstOrDefault(sc => sc.StudentId == studentId && sc.CourseId == courseId);
        }

        public void AddStudentCourse(StudentCourse studentCourse)
        {
            _context.StudentCourses.Add(studentCourse);
            _context.SaveChanges();
        }

        public void UpdateStudentCourse(StudentCourse studentCourse)
        {
            _context.StudentCourses.Update(studentCourse);
            _context.SaveChanges();
        }

        public void DeleteStudentCourse(int studentId, int courseId)
        {
            var studentCourse = _context.StudentCourses.FirstOrDefault(sc => sc.StudentId == studentId && sc.CourseId == courseId);
            if (studentCourse != null)
            {
                _context.StudentCourses.Remove(studentCourse);
                _context.SaveChanges();
            }
        }
    }
}
