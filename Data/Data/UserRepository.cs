using Microsoft.Data.SqlClient;
using Domain.Entities;

namespace Infrastructure.Data
{
    public class UserRepository
    {
        private readonly SqlConnection _connection;

        public UserRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<Employee> GetByUsernameAsync(string username, CancellationToken ct = default)
        {
            const string sql = "SELECT Id, Username, PasswordHash FROM Users WHERE Username = @Username";

            using var command = new SqlCommand(sql, _connection);
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

