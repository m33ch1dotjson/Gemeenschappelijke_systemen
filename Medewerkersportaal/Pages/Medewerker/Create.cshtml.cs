using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domain.Entities;
using Infrastructure.Data;

namespace Medewerkersportaal.Pages.Medewerker
{
    public class CreateModel : PageModel
    {
        private readonly EmployeeRepository _repository;

        public CreateModel(EmployeeRepository repository)
        {
            _repository = repository;
        }

        [BindProperty]
        public string full_name { get; set; } = string.Empty;

        [BindProperty]
        public string Username { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public async Task<IActionResult> OnPostAsync(CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return Page();

            var medewerker = new Employee
            {
                full_name = full_name,
                Username = Username
            };

            try
            {
                medewerker.SetPassword(Password);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("Password", ex.Message);
                return Page(); 
            }

            await _repository.AddAsync(medewerker, ct);
            return RedirectToPage("/Medewerker/Index");
        }
    }
}
