using TrainingCenter.Data;
using TrainingCenter.interfaces;
using TrainingCenter.Models;

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
            return [.. _context.Set<Student>()];
        }

        public Student GetStudent(int id)
        {
            var studentFromDB = _context.Set<Student>().Find(id) ?? throw new Exception("Student not found");
            return studentFromDB;
        }

        public void AddStudent(Student student)
        {
            _context.Set<Student>().Add(student);
            _context.SaveChanges();
        }

        public void UpdateStudent(Student student)
        {
            _context.Set<Student>().Update(student);
            _context.SaveChanges();
        }

        public void DeleteStudent(int id)
        {
            var studentFromDB = _context.Set<Student>().Find(id) ?? throw new Exception("Student not found");

            _context.Set<Student>().Remove(studentFromDB);
            _context.SaveChanges();
        }

        public List<StudentCourse> GetStudentCourses(int studentId)
        {
            return [.. _context.Set<StudentCourse>().Where(sc => sc.StudentId == studentId)];
        }

        public StudentCourse GetStudentCourse(int studentId, int courseId)
        {
            return _context.Set<StudentCourse>()
                .FirstOrDefault(sc => sc.StudentId == studentId && sc.CourseId == courseId);
        }

        public void AddStudentCourse(StudentCourse studentCourse)
        {
            _context.Set<StudentCourse>().Add(studentCourse);
            _context.SaveChanges();
        }

        public void UpdateStudentCourse(StudentCourse studentCourse)
        {
            _context.Set<StudentCourse>().Update(studentCourse);
            _context.SaveChanges();
        }

        public void DeleteStudentCourse(int studentId, int courseId)
        {
            var studentCourse = _context.Set<StudentCourse>()
                .FirstOrDefault(sc => sc.StudentId == studentId && sc.CourseId == courseId)
                ?? throw new Exception("Student course not found");
            
            _context.Set<StudentCourse>().Remove(studentCourse);
            _context.SaveChanges();
        }

    }
}
