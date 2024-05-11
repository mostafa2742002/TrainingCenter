using AutoMapper;
using TrainingCenter.DTO;
using TrainingCenter.Models;

namespace TrainingCenter.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Student, StudentDTO>().ReverseMap();
            CreateMap<Course, CourseDTO>().ReverseMap();
            CreateMap<StudentCourse, StudentCourseDTO>().ReverseMap();
            CreateMap<StudentDTO, Student>().ReverseMap();
            CreateMap<CourseDTO, Course>().ReverseMap();
            CreateMap<StudentCourseDTO, StudentCourse>().ReverseMap();

        }
    }
}
