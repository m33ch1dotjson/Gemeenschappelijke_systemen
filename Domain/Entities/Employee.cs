namespace Domain.Entities
{
    public class Employee
    {
        private string _fullName = string.Empty;
        private string _username = string.Empty;
        private string _passwordHash = string.Empty;

        public string GetFullName() => _fullName;
        public string GetUsername() => _username;
        public string GetPasswordHash() => _passwordHash;

        public Employee() { }

        public Employee(string username, string fullName)
        {
            SetUsername(username);
            SetFullName(fullName);
        }

        public void SetFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Naam mag niet leeg zijn.");

            _fullName = fullName;
        }

        public void SetUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Gebruikersnaam mag niet leeg zijn.");

            _username = username;
        }

        /// <summary>
        /// Sets the employee's password by hashing the provided plain text password using BCrypt.
        /// Ensures the password is not null, empty, or too short before hashing.
        /// </summary>
        /// <exception>
        /// Thrown if the password is null, whitespace, or shorter than 6 characters.
        /// </exception>
        public void SetPassword(string plainPassword)
        {
            if (string.IsNullOrWhiteSpace(plainPassword) || plainPassword.Length < 6)
                throw new ArgumentException("Wachtwoord moet minstens 6 tekens lang zijn.");

            _passwordHash = BCrypt.Net.BCrypt.HashPassword(plainPassword);
        }

        public void LoadPasswordHash(string hash)
        {
            _passwordHash = hash;
        }
    }
}
