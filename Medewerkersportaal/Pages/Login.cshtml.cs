using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Application.Services;          
using Domain.Entities;              

namespace Medewerkersportaal.Pages
{
    public class LoginModel : PageModel
    {
        private readonly LoginService _loginService;

        public LoginModel(LoginService loginService)
        {
            _loginService = loginService;
        }

        [BindProperty]
        [Required(ErrorMessage = "Gebruikersnaam is verplicht")]
        public string Username { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Wachtwoord is verplicht")]
        public string Password { get; set; } = string.Empty;

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync(CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Console.WriteLine($"Gebruikersnaam: {Username}, Wachtwoord: {Password}");
            var employee = await _loginService.ValidateLoginAsync(Username, Password, ct);

            if (employee == null)
            {
                Console.WriteLine("Login gefaald: employee == null");
                ErrorMessage = "Ongeldige gebruikersnaam of wachtwoord.";
                return Page();
            }

            return RedirectToPage("/Dashboard");
        }
    }
}
