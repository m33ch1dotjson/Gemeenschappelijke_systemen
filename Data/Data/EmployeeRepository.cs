using MySqlConnector;
using Domain.Entities;
using System.Data;
using Domain.Interfaces;

namespace Infrastructure.Data
{
    public class EmployeeRepository : IEmployeeRepository
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

            const string sql = "SELECT Username, full_name, PasswordHash FROM Employee WHERE Username = @Username";

            using var command = new MySqlCommand(sql, _connection);
            command.Parameters.AddWithValue("@Username", username);

            using var reader = await command.ExecuteReaderAsync(ct);
            if (await reader.ReadAsync(ct))
            {
                var employee = new Employee();
                employee.SetUsername(reader.GetString("Username"));
                employee.SetFullName(reader.GetString("full_name"));
                employee.LoadPasswordHash(reader.GetString("PasswordHash"));

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
                var emp = new Employee(
                    username: reader.GetString("Username"),
                    fullName: reader.GetString("full_name")
                );
                emp.LoadPasswordHash(reader.GetString("PasswordHash"));

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
            cmd.Parameters.AddWithValue("@full_name", medewerker.GetFullName());
            cmd.Parameters.AddWithValue("@Username", medewerker.GetUsername());
            cmd.Parameters.AddWithValue("@PasswordHash", medewerker.GetPasswordHash());

            await cmd.ExecuteNonQueryAsync(ct);
        }


    }
}

