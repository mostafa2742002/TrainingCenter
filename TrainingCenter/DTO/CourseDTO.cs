using TrainingCenter.Models;

namespace TrainingCenter.DTO
{
    public class CourseDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public required int Capacity { get; set; }
        public required decimal Cost { get; set; }
        public required string Status { get; set; }
    }

}
