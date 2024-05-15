using TrainingCenter.Models;

namespace TrainingCenter.DTO
{
    public class CourseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Capacity { get; set; }
        public decimal Cost { get; set; }
        public string Status { get; set; }
    }

}
