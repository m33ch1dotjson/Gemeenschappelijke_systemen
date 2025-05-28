using System.ComponentModel.DataAnnotations;

namespace CMS.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Gebruikersnaam is verplicht")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Wachtwoord is verplicht")]
        public string Password { get; set; } = string.Empty;

        public string? ErrorMessage { get; set; }
    }
}
