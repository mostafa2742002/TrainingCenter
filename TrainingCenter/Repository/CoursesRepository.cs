using Microsoft.EntityFrameworkCore;
using TrainingCenter.Data;
using TrainingCenter.Models;
using TrainingCenter.DTO;
using AutoMapper;
using TrainingCenter.interfaces;
using System.Collections.Specialized;

namespace TrainingCenter.Repository
{
    public class CoursesRepository : ICoursesRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CoursesRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<Course> GetCourses()
        {
            return _context.Courses.ToList();
        }

        public Course GetCourse(int id)
        {
            return _context.Courses.Find(id);
        }

        public void AddCourse(Course course)
        {
            _context.Courses.Add(course);
            _context.SaveChanges();
        }

        public void UpdateCourse(Course course)
        {
            _context.Courses.Update(course);
            _context.SaveChanges();
        }

        public void DeleteCourse(int id)
        {
            var course = _context.Courses.Find(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                _context.SaveChanges();
            }
        }

        public List<StudentCourse> GetCourseStudents(int courseId)
        {
            return _context.StudentCourses.Where(sc => sc.CourseId == courseId).ToList();
        }

        public StudentCourse GetCourseStudent(int courseId, int studentId)
        {
            return _context.StudentCourses.FirstOrDefault(sc => sc.CourseId == courseId && sc.StudentId == studentId);
        }

        public void AddCourseStudent(StudentCourse studentCourse)
        {
            _context.StudentCourses.Add(studentCourse);
            _context.SaveChanges();
        }

        public void UpdateCourseStudent(StudentCourse studentCourse)
        {
            _context.StudentCourses.Update(studentCourse);
            _context.SaveChanges();
        }

        public void DeleteCourseStudent(int courseId, int studentId)
        {
            var studentCourse = _context.StudentCourses.FirstOrDefault(sc => sc.CourseId == courseId && sc.StudentId == studentId);
            if (studentCourse != null)
            {
                _context.StudentCourses.Remove(studentCourse);
                _context.SaveChanges();
            }
        }

        public List<Course> GetCoursesAvailableForStudent(int studentId)
        {
            var studentCourses = _context.StudentCourses
                .Include(sc => sc.Course)
                .Where(sc => sc.StudentId == studentId)
                .ToList();

            var availableCourses = _context.Courses
                .Include(c => c.StudentCourses)
                .Where(c =>
                    c.StartDate > DateTime.Now &&
                    c.EndDate > DateTime.Now &&
                    c.Capacity > c.StudentCourses.Count &&
                    c.Status == "Not Started"
                ).ToList();

            for(int i = 0; i < availableCourses.Count; i++)
            {
                // convert the date to x/y/z format
                DateTime start = availableCourses[i].StartDate.Date;                
                DateTime end = availableCourses[i].EndDate.Date;
                string startString = start.ToString("yyyy/MM/dd");
                string endString = end.ToString("yyyy/MM/dd");
                availableCourses[i].StartDate = DateTime.Parse(startString);
                availableCourses[i].EndDate = DateTime.Parse(endString);

                for (int j = 0; j < studentCourses.Count; j++)
                {
                    DateTime dateTime = studentCourses[j].Course.StartDate.Date;
                    string dateTimeString = dateTime.ToString("yyyy/MM/dd");
                    studentCourses[j].Course.StartDate = DateTime.Parse(dateTimeString);
                    dateTime = studentCourses[j].Course.EndDate.Date;
                    dateTimeString = dateTime.ToString("yyyy/MM/dd");
                    studentCourses[j].Course.EndDate = DateTime.Parse(dateTimeString);
                    if (availableCourses[i].StartDate < studentCourses[j].Course.EndDate && availableCourses[i].EndDate > studentCourses[j].Course.StartDate)
                    {
                        availableCourses.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }
            return availableCourses;
        }

    }
}
