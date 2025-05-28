using System.ComponentModel.DataAnnotations;

namespace CMS.ViewModels
{
    public class EmployeeCreateViewModel
    {
        [Required(ErrorMessage = "Volledige naam is verplicht")]
        [Display(Name = "Volledige Naam")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Gebruikersnaam is verplicht")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Wachtwoord is verplicht")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
