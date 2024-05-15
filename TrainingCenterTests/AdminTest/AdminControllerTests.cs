using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using TrainingCenter.Controllers;
using TrainingCenter.DTO;
using Xunit;

namespace TrainingCenterTests.AdminTest
{
    public class AdminControllerTests
    {
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly AdminController _controller;

        public AdminControllerTests()
        {
            _mockConfig = new Mock<IConfiguration>();
            _mockConfig.Setup(config => config["Jwt:Key"]).Returns("A_Very_Secure_Key_With_At_Least_32_Chars");
            _mockConfig.Setup(config => config["Jwt:Issuer"]).Returns("TestIssuer");
            _mockConfig.Setup(config => config["Jwt:Audience"]).Returns("TestAudience");
            _controller = new AdminController(_mockConfig.Object);
        }

        [Fact]
        public void Login_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var loginRequest = new LoginDTO { Email = "Admin@Admin.com", Password = "Admin@123" };

            // Act
            var result = _controller.Login(loginRequest) as OkObjectResult;
            var resultValue = result?.Value as LoginResponse;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(resultValue?.Token);
            Assert.NotNull(resultValue?.Expiration);

            // Validate the token
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(resultValue.Token) as JwtSecurityToken;

            Assert.Equal("TestIssuer", jsonToken.Issuer);
            Assert.Equal("TestAudience", jsonToken.Audiences.First());
        }

        [Fact]
        public void Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var loginRequest = new LoginDTO { Email = "wrong@Admin.com", Password = "wrongPassword" };

            // Act
            var result = _controller.Login(loginRequest);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public void Login_NullRequest_ReturnsBadRequest()
        {
            // Act
            var result = _controller.Login(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Login request is null.", badRequestResult.Value);
        }
    }
}
