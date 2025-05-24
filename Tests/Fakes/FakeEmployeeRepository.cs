using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;

namespace Medewerkersportaal.Tests.Fakes
{
    public class FakeEmployeeRepository : IEmployeeRepository
    {
        private readonly List<Employee> _employees = new();

        public Task AddAsync(Employee medewerker, CancellationToken ct = default)
        {
            _employees.Add(medewerker);
            return Task.CompletedTask;
        }

        public Task<List<Employee>> GetAllAsync(CancellationToken ct = default)
        {
            return Task.FromResult(_employees);
        }

        public Task<Employee?> GetByUsernameAsync(string username, CancellationToken ct = default)
        {
            var employee = _employees.FirstOrDefault(e => e.GetUsername() == username);
            return Task.FromResult<Employee?>(employee);
        }
    }
}
