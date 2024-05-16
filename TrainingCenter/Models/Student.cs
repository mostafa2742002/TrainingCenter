using System;
using System.Collections.Generic;

namespace TrainingCenter.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Governorate { get; set; }
        public DateTime BirthDate { get; set; }
        public List<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    }
}
