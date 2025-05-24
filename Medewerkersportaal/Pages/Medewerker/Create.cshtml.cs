using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domain.Entities;
using Infrastructure.Data;
using Domain.Interfaces;

namespace Medewerkersportaal.Pages.Medewerker
{
    public class CreateModel : PageModel
    {
        private readonly IEmployeeRepository _repository;

        public CreateModel(IEmployeeRepository repository)
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

            var medewerker = new Employee();

            try
            {
                medewerker.SetUsername(Username);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(nameof(Username), ex.Message);
            }

            try
            {
                medewerker.SetFullName(full_name);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(nameof(full_name), ex.Message);
            }


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
