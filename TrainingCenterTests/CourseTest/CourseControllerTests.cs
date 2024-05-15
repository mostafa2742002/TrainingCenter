using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenter.Controllers;
using TrainingCenter.interfaces;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using TrainingCenter.Controllers;
using TrainingCenter.DTO;
using TrainingCenter.Models;
namespace TrainingCenterTests.CourseTest
{
    public class CoursesControllerTests
    {
        private readonly Mock<ICoursesRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CoursesController _controller;

        public CoursesControllerTests()
        {
            _mockRepo = new Mock<ICoursesRepository>();
            _mockMapper = new Mock<IMapper>();
            _controller = new CoursesController(_mockRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public void GetCourses_ReturnsCoursesList()
        {
            // Arrange
            var courses = new List<Course> { new Course { Id = 1, Name = "Course1" } };
            var courseDtos = new List<CourseDTO> { new CourseDTO { Id = 1, Name = "Course1" } };

            _mockRepo.Setup(repo => repo.GetCourses()).Returns(courses);
            _mockMapper.Setup(m => m.Map<List<CourseDTO>>(courses)).Returns(courseDtos);

            // Act
            var result = _controller.GetCourses() as OkObjectResult;
            var resultCourses = result?.Value as List<CourseDTO>;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Single(resultCourses);
            Assert.Equal("Course1", resultCourses[0].Name);
        }

        [Fact]
        public void GetCourses_ReturnsNotFound_WhenNoCourses()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetCourses()).Returns(new List<Course>());

            // Act
            var result = _controller.GetCourses();

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void GetCourse_ReturnsCourseById()
        {
            // Arrange
            var course = new Course { Id = 1, Name = "Course1" };
            var courseDto = new CourseDTO { Id = 1, Name = "Course1" };

            _mockRepo.Setup(repo => repo.GetCourse(1)).Returns(course);
            _mockMapper.Setup(m => m.Map<CourseDTO>(course)).Returns(courseDto);

            // Act
            var result = _controller.GetCourse(1) as OkObjectResult;
            var resultCourse = result?.Value as CourseDTO;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Course1", resultCourse.Name);
        }

        [Fact]
        public void GetCourse_ReturnsNotFound_WhenCourseDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetCourse(1)).Returns((Course)null);

            // Act
            var result = _controller.GetCourse(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void AddCourse_ReturnsCreatedResponse_WhenCourseIsValid()
        {
            // Arrange
            var courseDto = new CourseDTO { Name = "Course1", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), Capacity = 10, Cost = 100m, Status = "Active" };
            var course = new Course { Id = 1, Name = "Course1" };

            _mockRepo.Setup(repo => repo.GetCourses()).Returns(new List<Course>());
            _mockMapper.Setup(m => m.Map<Course>(courseDto)).Returns(course);
            _mockRepo.Setup(repo => repo.AddCourse(course)).Verifiable();

            // Act
            var result = _controller.AddCourse(courseDto) as CreatedAtActionResult;
            var resultCourse = result?.Value as Course;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal("Course1", resultCourse.Name);
            _mockRepo.Verify(repo => repo.AddCourse(course), Times.Once);
        }

        [Fact]
        public void AddCourse_ReturnsBadRequest_WhenCourseIsInvalid()
        {
            // Arrange
            var courseDto = new CourseDTO { Name = "Course1", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(-1), Capacity = 10, Cost = 100m, Status = "Active" };

            _controller.ModelState.AddModelError("EndDate", "Start date must be before end date.");

            // Act
            var result = _controller.AddCourse(courseDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdateCourse_ReturnsNoContent_WhenCourseIsUpdated()
        {
            // Arrange
            var courseDto = new CourseDTO { Id = 1, Name = "Updated Course", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), Capacity = 10, Cost = 100m, Status = "Active" };
            var course = new Course { Id = 1, Name = "Course1" };

            _mockRepo.Setup(repo => repo.GetCourse(1)).Returns(course);
            _mockMapper.Setup(m => m.Map(courseDto, course)).Verifiable();
            _mockRepo.Setup(repo => repo.UpdateCourse(course)).Verifiable();

            // Act
            var result = _controller.UpdateCourse(1, courseDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockMapper.Verify(m => m.Map(courseDto, course), Times.Once);
            _mockRepo.Verify(repo => repo.UpdateCourse(course), Times.Once);
        }

        [Fact]
        public void UpdateCourse_ReturnsNotFound_WhenCourseDoesNotExist()
        {
            // Arrange
            var courseDto = new CourseDTO { Id = 1, Name = "Updated Course", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), Capacity = 10, Cost = 100m, Status = "Active" };

            _mockRepo.Setup(repo => repo.GetCourse(1)).Returns((Course)null);

            // Act
            var result = _controller.UpdateCourse(1, courseDto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void DeleteCourse_ReturnsNoContent_WhenCourseIsDeleted()
        {
            // Arrange
            var course = new Course { Id = 1, Name = "Course1" };

            _mockRepo.Setup(repo => repo.GetCourse(1)).Returns(course);
            _mockRepo.Setup(repo => repo.DeleteCourse(1)).Verifiable();

            // Act
            var result = _controller.DeleteCourse(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockRepo.Verify(repo => repo.DeleteCourse(1), Times.Once);
        }

        [Fact]
        public void DeleteCourse_ReturnsNotFound_WhenCourseDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetCourse(1)).Returns((Course)null);

            // Act
            var result = _controller.DeleteCourse(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}

