using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domain.Interfaces;

namespace Medewerkersportaal.Pages.Medewerker
{
    public class IndexModel : PageModel
    {
        private readonly IEmployeeRepository _repository;

        public IndexModel(IEmployeeRepository repository)
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
