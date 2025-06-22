using Xunit;
using Application.Services;
using Domain.Entities;
using Medewerkersportaal.Tests.Fakes;

public class LoginServiceTests
{
    [Fact]
    public async Task LoginMetCorrectWachtwoord_ReturntEmployee()
    {
        // Arrange: create a fake repository and an employee with a known password
        var repo = new FakeEmployeeRepository();
        var emp = new Employee("mika", "Mika Kleinbergen");
        emp.SetPassword("geheim123");
        await repo.AddAsync(emp);

        // Act: attempt to validate login with correct username and password
        var service = new LoginService(repo);
        var result = await service.ValidateLoginAsync("mika", "geheim123");

        // Assert: expect a non-null result, meaning login was successful
        Assert.NotNull(result);
    }
}
