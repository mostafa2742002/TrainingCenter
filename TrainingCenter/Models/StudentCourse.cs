namespace TrainingCenter.Models
{
    public class StudentCourse
    {
        
        public int Id { get; set; }
        public required int StudentId { get; set; } // Foreign Key
        public required int CourseId { get; set; } // Foreign Key
        public Student Student { get; set; } // Navigation property
        public Course Course { get; set; } // Navigation property
        public required DateTime RegistrationDate { get; set; } // Date of registration
        public decimal Grade { get; set; } // Student's grade in the course
        public required string Status { get; set; } // Status of the student in the course 
    }
}
