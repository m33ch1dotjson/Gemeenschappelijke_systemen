using BCrypt.Net;
using Domain.Entities;

namespace Tests
{
    public class Password
    {
        [Fact]
        public void SetPassword_ThrowsException_WhenTooShort()
        {
            var employee = new Employee("Mika","Mika1401");
            Assert.Throws<ArgumentException>(() => employee.SetPassword("123"));
        }

        [Fact]
        public void SetPassword_GeneratesHash_ForValidPassword()
        {
            var employee = new Employee("Mika","Mika1401");
            employee.SetPassword("veilig123");

            Assert.False(string.IsNullOrEmpty(employee.GetPasswordHash()));
            Assert.True(BCrypt.Net.BCrypt.Verify("veilig123", employee.GetPasswordHash()));
        }

    }
}

