using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
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

        [Fact]
        public void ValidateToken_ValidToken_ReturnsOk()
        {
            // Arrange
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("your-secret-key-here");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[] { new System.Security.Claims.Claim("sub", "test") }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = "your-issuer",
                Audience = "your-audience"
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var validToken = tokenHandler.WriteToken(token);

            // Act
            var result = _controller.ValidateToken(validToken);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void ValidateToken_InvalidToken_ReturnsUnauthorized()
        {
            // Arrange
            var invalidToken = "invalid-token";

            // Act
            var result = _controller.ValidateToken(invalidToken);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public void ValidateToken_ExpiredToken_ReturnsUnauthorized()
        {
            // Arrange
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("your-secret-key-here");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[] { new System.Security.Claims.Claim("sub", "test") }),
                Expires = DateTime.UtcNow.AddMinutes(-30), // Expired token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = "your-issuer",
                Audience = "your-audience"
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var expiredToken = tokenHandler.WriteToken(token);

            // Act
            var result = _controller.ValidateToken(expiredToken);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

    }
}
