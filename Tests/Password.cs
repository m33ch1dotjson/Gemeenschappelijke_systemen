using BCrypt.Net;
using Domain.Entities;

namespace Tests
{
    public class Password
    {
        [Fact]
        public void SetPassword_ThrowsException_WhenTooShort()
        {
            var employee = new Employee();
            Assert.Throws<ArgumentException>(() => employee.SetPassword("123"));
        }

        [Fact]
        public void SetPassword_GeneratesHash_ForValidPassword()
        {
            var employee = new Employee();
            employee.SetPassword("veilig123");

            Assert.False(string.IsNullOrEmpty(employee.PasswordHash));
            Assert.True(BCrypt.Net.BCrypt.Verify("veilig123", employee.PasswordHash));
        }

    }
}

