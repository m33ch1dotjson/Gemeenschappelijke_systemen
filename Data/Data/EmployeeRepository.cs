using MySqlConnector;
using Domain.Entities;

namespace Infrastructure.Data
{
    public class EmployeeRepository
    {
        private readonly MySqlConnection _connection;

        public EmployeeRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<Employee> GetByUsernameAsync(string username, CancellationToken ct = default)
        {
            if (_connection.State != System.Data.ConnectionState.Open)
                await _connection.OpenAsync(ct);

            const string sql = "SELECT Id, Username, PasswordHash FROM Employee WHERE Username = @Username";

            using var command = new MySqlCommand(sql, _connection);
            command.Parameters.AddWithValue("@Username", username);

            using var reader = await command.ExecuteReaderAsync(ct);
            if (await reader.ReadAsync(ct))
            {
                return new Employee
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    PasswordHash = reader.GetString(2)
                };
            }

            return null;
        }
    }
}

