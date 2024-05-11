using TrainingCenter.Models;

namespace TrainingCenter.interfaces
{
    public interface IStudentRepository
    {
        
        List<Student> GetStudents();
        Student GetStudent(int id);
        void AddStudent(Student student);
        void UpdateStudent(Student student);
        void DeleteStudent(int id);
        List<StudentCourse> GetStudentCourses(int studentId);
        StudentCourse GetStudentCourse(int studentId, int courseId);
        void AddStudentCourse(StudentCourse studentCourse);
        void UpdateStudentCourse(StudentCourse studentCourse);
        void DeleteStudentCourse(int studentId, int courseId);
        
    }
}
