using Domain.Entities;
using Infrastructure.Data;
using Domain.Interfaces;


namespace Application.Services
{
    /// <summary>
    /// Handles login validation for employees.
    /// </summary>
    public class LoginService
    {
        private readonly IEmployeeRepository _employeeRepository;

        /// <summary>
        /// Constructor that injects the employee repository.
        /// </summary>
        public LoginService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Validates a username and password against the database.
        /// </summary>
        /// <param name="username">The input username.</param>
        /// <param name="password">The input password.</param>
        /// <param name="ct">Optional cancellation token.</param>
        /// <returns>The matching employee if valid, otherwise null.</returns>
        public async Task<Employee?> ValidateLoginAsync(string username, string password, CancellationToken ct = default)
        {

            // Attempt to retrieve the employee by username
            var employee = await _employeeRepository.GetByUsernameAsync(username, ct);
            if (employee == null)
                return null;

            // Validate the password using BCrypt
            bool isValid = BCrypt.Net.BCrypt.Verify(password, employee.GetPasswordHash());

            // Return employee if login is valid, otherwise null
            return isValid ? employee : null;
        }
    }
}
