using System;
using System.Collections.Generic;

namespace TrainingCenter.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Capacity { get; set; }
        public decimal Cost { get; set; }
        public string Status { get; set; }
        public List<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    }
}
