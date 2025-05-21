namespace Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string full_name { get; set; }
        public string Username { get; set; } = default!;
        public string PasswordHash { get; private set; } = default!;

        public void SetPassword(string plainPassword)
        {
            if (string.IsNullOrWhiteSpace(plainPassword) || plainPassword.Length < 6)
                throw new ArgumentException("Wachtwoord moet minstens 6 tekens lang zijn.");

            PasswordHash = BCrypt.Net.BCrypt.HashPassword(plainPassword);
        }

        public void LoadPasswordHash(string hash)
        {
            PasswordHash = hash;
        }
    }
}
