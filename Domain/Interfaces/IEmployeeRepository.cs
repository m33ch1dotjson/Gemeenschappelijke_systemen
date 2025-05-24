using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<Employee?> GetByUsernameAsync(string username, CancellationToken ct = default);
        Task<List<Employee>> GetAllAsync(CancellationToken ct = default);
        Task AddAsync(Employee medewerker, CancellationToken ct = default);
    }
}
