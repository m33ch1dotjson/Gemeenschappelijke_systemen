using Domain.Entities;
using Infrastructure.Data;
using Domain.Interfaces;


namespace Application.Services
{
    public class LoginService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public LoginService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<Employee?> ValidateLoginAsync(string username, string password, CancellationToken ct = default)
        {
            Console.WriteLine($"[LoginService] Gebruikersnaam: {username}");

            var employee = await _employeeRepository.GetByUsernameAsync(username, ct);
            if (employee == null)
                return null;

            Console.WriteLine("[LoginService] Gebruiker gevonden, wachtwoord controleren...");

            bool isValid = BCrypt.Net.BCrypt.Verify(password, employee.GetPasswordHash());
            Console.WriteLine($"[LoginService] Wachtwoord geldig: {isValid}");
            var hash = BCrypt.Net.BCrypt.HashPassword("admin");
            Console.WriteLine(hash);

            return isValid ? employee : null;
        }
    }
}
