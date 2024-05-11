using TrainingCenter.Models;

namespace TrainingCenter.DTO
{
    public class StudentCourseDTO
    {
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public decimal Grade { get; set; }
        public string Status { get; set; }
    }
}
