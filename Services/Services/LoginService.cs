using Domain.Entities;
using Infrastructure.Data;


namespace Application.Services
{
    public class LoginService
    {
        private readonly UserRepository _userRepository;

        public LoginService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Employee?> ValidateLoginAsync(string username, string password, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByUsernameAsync(username, ct);
            if (user == null)
                return null;

            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            return isValid ? user : null;
        }
    }
}
