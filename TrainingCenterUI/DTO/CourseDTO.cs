namespace TrainingCenterUI.DTO
{
        public class CourseDTO
        {
            public int Id { get; set; }
            public required string Name { get; set; }
            public DateTime StartDate { get; set; }  // Changed from string to DateTime
            public DateTime EndDate { get; set; }    // Changed from string to DateTime
            public required int Capacity { get; set; }
            public required decimal Cost { get; set; }
            public required string Status { get; set; }
        }
}
