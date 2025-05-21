using MySqlConnector;
using Domain.Entities;
using System.Data;

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
                var employee = new Employee
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                };
                employee.LoadPasswordHash(reader.GetString(2)); 
                return employee;
            }

            return null;
        }

        public async Task<List<Employee>> GetAllAsync(CancellationToken ct)
        {
            if (_connection.State != ConnectionState.Open)
                await _connection.OpenAsync(ct);

            const string sql = "SELECT Id, full_name, Username, PasswordHash FROM Employee";

            using var cmd = new MySqlCommand(sql, _connection);
            using var reader = await cmd.ExecuteReaderAsync(ct);

            var list = new List<Employee>();
            while (await reader.ReadAsync(ct))
            {
                var emp = new Employee
                {
                    Id = reader.GetInt32("Id"),
                    full_name = reader.GetString("full_name"),
                    Username = reader.GetString("Username"),
                };
                list.Add(emp);
            }

            return list;
        }

        public async Task AddAsync(Employee medewerker, CancellationToken ct)
        {
            if (_connection.State != ConnectionState.Open)
                await _connection.OpenAsync(ct);

            const string sql = """
            INSERT INTO Employee (full_name, Username, PasswordHash)
            VALUES (@full_name, @Username, @PasswordHash)
            """;

            using var cmd = new MySqlCommand(sql, _connection);
            cmd.Parameters.AddWithValue("@full_name", medewerker.full_name);
            cmd.Parameters.AddWithValue("@Username", medewerker.Username);
            cmd.Parameters.AddWithValue("@PasswordHash", medewerker.PasswordHash);

            await cmd.ExecuteNonQueryAsync(ct);
        }


    }
}

