using Xunit;
using Application.Services;
using Domain.Entities;
using Medewerkersportaal.Tests.Fakes;

public class LoginServiceTests
{
    [Fact]
    public async Task LoginMetCorrectWachtwoord_ReturntEmployee()
    {
        var repo = new FakeEmployeeRepository();
        var emp = new Employee("mika", "Mika Kleinbergen");
        emp.SetPassword("geheim123");
        await repo.AddAsync(emp);

        var service = new LoginService(repo);
        var result = await service.ValidateLoginAsync("mika", "geheim123");

        Assert.NotNull(result);
    }
}
