using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using TrainingCenter.Controllers;
using TrainingCenter.DTO;
using TrainingCenter.Models;
using TrainingCenter.interfaces;

namespace TrainingCenter.StudentTest
{
    public class StudentsControllerTests
    {
        private readonly Mock<IStudentRepository> _mockStudentRepo;
        private readonly Mock<ICoursesRepository> _mockCourseRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly StudentsController _controller;

        public StudentsControllerTests()
        {
            _mockStudentRepo = new Mock<IStudentRepository>();
            _mockCourseRepo = new Mock<ICoursesRepository>();
            _mockMapper = new Mock<IMapper>();
            _controller = new StudentsController(_mockStudentRepo.Object, _mockMapper.Object, _mockCourseRepo.Object);
        }

        [Fact]
        public void GetStudents_ReturnsStudentsList()
        {
            // Arrange
            var students = new List<Student> { new Student { Id = 1, Name = "Student1" } };
            var studentDtos = new List<StudentDTO> { new StudentDTO { Id = 1, Name = "Student1" } };

            _mockStudentRepo.Setup(repo => repo.GetStudents()).Returns(students);
            _mockMapper.Setup(m => m.Map<List<StudentDTO>>(students)).Returns(studentDtos);

            // Act
            var result = _controller.GetStudents() as OkObjectResult;
            var resultStudents = result?.Value as List<StudentDTO>;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Single(resultStudents);
            Assert.Equal("Student1", resultStudents[0].Name);
        }

        [Fact]
        public void GetStudents_ReturnsNotFound_WhenNoStudents()
        {
            // Arrange
            _mockStudentRepo.Setup(repo => repo.GetStudents()).Returns(new List<Student>());

            // Act
            var result = _controller.GetStudents();

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void GetStudent_ReturnsStudentById()
        {
            // Arrange
            var student = new Student { Id = 1, Name = "Student1" };
            var studentDto = new StudentDTO { Id = 1, Name = "Student1" };

            _mockStudentRepo.Setup(repo => repo.GetStudent(1)).Returns(student);
            _mockMapper.Setup(m => m.Map<StudentDTO>(student)).Returns(studentDto);

            // Act
            var result = _controller.GetStudent(1) as OkObjectResult;
            var resultStudent = result?.Value as StudentDTO;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Student1", resultStudent.Name);
        }

        [Fact]
        public void GetStudent_ReturnsNotFound_WhenStudentDoesNotExist()
        {
            // Arrange
            _mockStudentRepo.Setup(repo => repo.GetStudent(1)).Returns((Student)null);

            // Act
            var result = _controller.GetStudent(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void AddStudent_ReturnsCreatedResponse_WhenStudentIsValid()
        {
            // Arrange
            var studentDto = new StudentDTO { Name = "Student1", Email = "student1@example.com", Governorate = "Gov1", BirthDate = DateTime.Now };
            var student = new Student { Id = 1, Name = "Student1" };

            _mockMapper.Setup(m => m.Map<Student>(studentDto)).Returns(student);
            _mockStudentRepo.Setup(repo => repo.AddStudent(student)).Verifiable();

            // Act
            var result = _controller.AddStudent(studentDto) as CreatedAtActionResult;
            var resultStudent = result?.Value as StudentDTO;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal("Student1", resultStudent.Name);
            _mockStudentRepo.Verify(repo => repo.AddStudent(student), Times.Once);
        }

        [Fact]
        public void AddStudent_ReturnsBadRequest_WhenStudentIsInvalid()
        {
            // Arrange
            var studentDto = new StudentDTO { Name = "", Email = "student1@example.com", Governorate = "Gov1", BirthDate = DateTime.Now };

            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = _controller.AddStudent(studentDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdateStudent_ReturnsNoContent_WhenStudentIsUpdated()
        {
            // Arrange
            var studentDto = new StudentDTO { Id = 1, Name = "Updated Student", Email = "student1@example.com", Governorate = "Gov1", BirthDate = DateTime.Now };
            var student = new Student { Id = 1, Name = "Student1" };

            _mockStudentRepo.Setup(repo => repo.GetStudent(1)).Returns(student);
            _mockMapper.Setup(m => m.Map(studentDto, student)).Verifiable();
            _mockStudentRepo.Setup(repo => repo.UpdateStudent(student)).Verifiable();

            // Act
            var result = _controller.UpdateStudent(1, studentDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockMapper.Verify(m => m.Map(studentDto, student), Times.Once);
            _mockStudentRepo.Verify(repo => repo.UpdateStudent(student), Times.Once);
        }

        [Fact]
        public void UpdateStudent_ReturnsNotFound_WhenStudentDoesNotExist()
        {
            // Arrange
            var studentDto = new StudentDTO { Id = 1, Name = "Updated Student", Email = "student1@example.com", Governorate = "Gov1", BirthDate = DateTime.Now };

            _mockStudentRepo.Setup(repo => repo.GetStudent(1)).Returns((Student)null);

            // Act
            var result = _controller.UpdateStudent(1, studentDto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void DeleteStudent_ReturnsNoContent_WhenStudentIsDeleted()
        {
            // Arrange
            var student = new Student { Id = 1, Name = "Student1" };

            _mockStudentRepo.Setup(repo => repo.GetStudent(1)).Returns(student);
            _mockStudentRepo.Setup(repo => repo.DeleteStudent(1)).Verifiable();

            // Act
            var result = _controller.DeleteStudent(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockStudentRepo.Verify(repo => repo.DeleteStudent(1), Times.Once);
        }

        [Fact]
        public void DeleteStudent_ReturnsNotFound_WhenStudentDoesNotExist()
        {
            // Arrange
            _mockStudentRepo.Setup(repo => repo.GetStudent(1)).Returns((Student)null);

            // Act
            var result = _controller.DeleteStudent(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void GetStudentCourses_ReturnsCoursesList()
        {
            // Arrange
            var studentCourses = new List<StudentCourse> { new StudentCourse { StudentId = 1, CourseId = 1 , RegistrationDate = DateTime.Now, Grade = 0, Status = "Active" } };
            var studentCourseDtos = new List<StudentCourseDTO> { new StudentCourseDTO { StudentId = 1, CourseId = 1 } };

            _mockStudentRepo.Setup(repo => repo.GetStudentCourses(1)).Returns(studentCourses);
            _mockMapper.Setup(m => m.Map<List<StudentCourseDTO>>(studentCourses)).Returns(studentCourseDtos);

            // Act
            var result = _controller.GetStudentCourses(1) as OkObjectResult;
            var resultCourses = result?.Value as List<StudentCourseDTO>;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Single(resultCourses);
            Assert.Equal(1, resultCourses[0].CourseId);
        }

        [Fact]
        public void GetStudentCourses_ReturnsEmptyList_WhenNoCourses()
        {
            // Arrange
            _mockStudentRepo.Setup(repo => repo.GetStudentCourses(1)).Returns(new List<StudentCourse>());

            // Act
            var result = _controller.GetStudentCourses(1) as OkObjectResult;
            var resultCourses = result?.Value as List<StudentCourse>;

            // Assert
            Assert.NotNull(result);
            Assert.Empty(resultCourses);
        }

        [Fact]
        public void GetStudentCourse_ReturnsCourseById()
        {
            // Arrange
            var studentCourse = new StudentCourse { StudentId = 1, CourseId = 1 , RegistrationDate = DateTime.Now, Grade = 0, Status = "Active" };
            var studentCourseDto = new StudentCourseDTO { StudentId = 1, CourseId = 1 };

            _mockStudentRepo.Setup(repo => repo.GetStudentCourse(1, 1)).Returns(studentCourse);
            _mockMapper.Setup(m => m.Map<StudentCourseDTO>(studentCourse)).Returns(studentCourseDto);

            // Act
            var result = _controller.GetStudentCourse(1, 1) as OkObjectResult;
            var resultCourse = result?.Value as StudentCourseDTO;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(1, resultCourse.CourseId);
        }

        [Fact]
        public void GetStudentCourse_ReturnsNull_WhenCourseDoesNotExist()
        {
            // Arrange
            _mockStudentRepo.Setup(repo => repo.GetStudentCourse(1, 1)).Returns((StudentCourse)null);

            // Act
            var result = _controller.GetStudentCourse(1, 1) as OkObjectResult;

            // Assert
            Assert.Null(result?.Value);
        }

        [Fact]
        public void AddStudentCourse_ReturnsCreatedResponse_WhenCourseIsValid()
        {
            // Arrange
            var studentCourseDto = new StudentCourseDTO { StudentId = 1, CourseId = 1 };
            var studentCourse = new StudentCourse { StudentId = 1, CourseId = 1, RegistrationDate = DateTime.Now, Grade = 0, Status = "Active" };
            var student = new Student { Id = 1, Name = "Student1" };
            var course = new Course { Id = 1, Name = "Course1" };

            _mockStudentRepo.Setup(repo => repo.GetStudent(1)).Returns(student);
            _mockCourseRepo.Setup(repo => repo.GetCourse(1)).Returns(course);
            _mockStudentRepo.Setup(repo => repo.AddStudentCourse(studentCourse)).Verifiable();

            // Act
            var result = _controller.AddStudentCourse(1, studentCourseDto) as CreatedAtActionResult;
            var resultCourse = result?.Value as StudentCourseDTO;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal(1, resultCourse.CourseId);
            _mockStudentRepo.Verify(repo => repo.AddStudentCourse(It.IsAny<StudentCourse>()), Times.Once);
        }

        [Fact]
        public void AddStudentCourse_ReturnsBadRequest_WhenCourseIsInvalid()
        {
            // Arrange
            var studentCourseDto = new StudentCourseDTO { StudentId = 1, CourseId = 1 };

            _controller.ModelState.AddModelError("CourseId", "Required");

            // Act
            var result = _controller.AddStudentCourse(1, studentCourseDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void AddStudentCourse_ReturnsNotFound_WhenStudentOrCourseDoesNotExist()
        {
            // Arrange
            var studentCourseDto = new StudentCourseDTO { StudentId = 1, CourseId = 1 };

            _mockStudentRepo.Setup(repo => repo.GetStudent(1)).Returns((Student)null);
            _mockCourseRepo.Setup(repo => repo.GetCourse(1)).Returns((Course)null);

            // Act
            var resultStudent = _controller.AddStudentCourse(1, studentCourseDto);
            var resultCourse = _controller.AddStudentCourse(1, studentCourseDto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(resultStudent);
            Assert.IsType<NotFoundObjectResult>(resultCourse);
        }

        [Fact]
        public void UpdateStudentCourse_ReturnsNoContent_WhenStudentCourseIsUpdated()
        {
            // Arrange
            var studentCourseDto = new StudentCourseDTO { StudentId = 1, CourseId = 1 };
            var studentCourse = new StudentCourse { StudentId = 1, CourseId = 1 , RegistrationDate = DateTime.Now, Grade = 0, Status = "Active" };

            _mockStudentRepo.Setup(repo => repo.GetStudentCourse(1, 1)).Returns(studentCourse);
            _mockMapper.Setup(m => m.Map(studentCourseDto, studentCourse)).Verifiable();
            _mockStudentRepo.Setup(repo => repo.UpdateStudentCourse(studentCourse)).Verifiable();

            // Act
            var result = _controller.UpdateStudentCourse(1, 1, studentCourseDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockMapper.Verify(m => m.Map(studentCourseDto, studentCourse), Times.Once);
            _mockStudentRepo.Verify(repo => repo.UpdateStudentCourse(studentCourse), Times.Once);
        }

        [Fact]
        public void UpdateStudentCourse_ReturnsNotFound_WhenStudentCourseDoesNotExist()
        {
            // Arrange
            var studentCourseDto = new StudentCourseDTO { StudentId = 1, CourseId = 1 };

            _mockStudentRepo.Setup(repo => repo.GetStudentCourse(1, 1)).Returns((StudentCourse)null);

            // Act
            var result = _controller.UpdateStudentCourse(1, 1, studentCourseDto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void DeleteStudentCourse_ReturnsNoContent_WhenStudentCourseIsDeleted()
        {
            // Arrange
            var studentCourse = new StudentCourse { StudentId = 1, CourseId = 1, RegistrationDate = DateTime.Now, Grade = 0, Status = "Active" };

            _mockStudentRepo.Setup(repo => repo.GetStudentCourse(1, 1)).Returns(studentCourse);
            _mockStudentRepo.Setup(repo => repo.DeleteStudentCourse(1, 1)).Verifiable();

            // Act
            var result = _controller.DeleteStudentCourse(1, 1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockStudentRepo.Verify(repo => repo.DeleteStudentCourse(1, 1), Times.Once);
        }

        [Fact]
        public void DeleteStudentCourse_ReturnsNotFound_WhenStudentCourseDoesNotExist()
        {
            // Arrange
            _mockStudentRepo.Setup(repo => repo.GetStudentCourse(1, 1)).Returns((StudentCourse)null);

            // Act
            var result = _controller.DeleteStudentCourse(1, 1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
