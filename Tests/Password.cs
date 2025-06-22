using Application.Services;
using BCrypt.Net;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Xunit;

namespace Tests
{
    public class Password
    {
        [Fact]
        public void SetPassword_ThrowsException_WhenTooShort()
        {
            // Arrange: create an employee
            var employee = new Employee("Mika", "Mika1401");

            // Act & Assert: expect an ArgumentException when password is too short
            Assert.Throws<ArgumentException>(() => employee.SetPassword("123"));
        }

        [Fact]
        public void SetPassword_GeneratesHash_ForValidPassword()
        {
            // Arrange: create an employee and set a valid password
            var employee = new Employee("Mika", "Mika1401");
            employee.SetPassword("veilig123");

            // Assert: password hash should not be null or empty
            Assert.False(string.IsNullOrEmpty(employee.GetPasswordHash()));

            // Assert: the hash should match the original password using BCrypt verification
            Assert.True(BCrypt.Net.BCrypt.Verify("veilig123", employee.GetPasswordHash()));
        }

        [Fact]
        public void Login_WithDifferentCasingInUsername_StillSucceeds()
        {
            // Arrange: create an employee and set a valid password
            var employee = new Employee("Mika", "Mika Kleinbergen");
            employee.SetPassword("veilig123");

            // Create a mock repository
            var mockRepo = new Mock<IEmployeeRepository>();

            // Setup the repository to return the employee when searched by "mika"
            // Use It.IsAny<CancellationToken>() because the method expects one
            mockRepo.Setup(r => r.GetByUsernameAsync("mika", It.IsAny<CancellationToken>()))
                    .ReturnsAsync(employee);

            // Create the LoginService using the mocked repository
            var service = new LoginService(mockRepo.Object);

            // Act: attempt to login using a different casing for the username
            var result = service.ValidateLoginAsync("MIKA", "veilig123").Result;

            // Assert: the result should not be null, meaning login succeeded
            Assert.NotNull(result);
        }

    }
}
