using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Medewerkersportaal.Pages.Medewerker
{
    public class IndexModel : PageModel
    {
        private readonly EmployeeRepository _repository;

        public IndexModel(EmployeeRepository repository)
        {
            _repository = repository;
        }

        public List<Employee> Medewerkers { get; set; } = new();

        public async Task OnGetAsync(CancellationToken ct)
        {
            Medewerkers = await _repository.GetAllAsync(ct);
        }
    }
}
