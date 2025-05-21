namespace Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }

        public string Username { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;

    }
}
