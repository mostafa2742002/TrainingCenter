using TrainingCenter.Models;

namespace TrainingCenter.interfaces
{
    public interface ICoursesRepository
    {
        
        List<Course> GetCourses();
        Course GetCourse(int id);
        void AddCourse(Course course);
        void UpdateCourse(Course course);
        void DeleteCourse(int id);
        List<StudentCourse> GetCourseStudents(int courseId);
        StudentCourse GetCourseStudent(int courseId, int studentId);
        void AddCourseStudent(StudentCourse studentCourse);
        void UpdateCourseStudent(StudentCourse studentCourse);
        void DeleteCourseStudent(int courseId, int studentId);
        List<Course> GetCoursesAvailableForStudent(int studentId);
    }
}
