using Microsoft.AspNetCore.Mvc;
using CMS.ViewModels;
using Application.Services;
using CMS.Models;

namespace CMS.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginService _loginService;

        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

        /// <summary>
        /// Handles the POST request for the login form.
        /// Validates the input model, checks the user credentials via the login service,
        /// sets the current user in session if valid, and redirects to the dashboard.
        /// If validation fails or credentials are incorrect, the login view is shown again with an error message.
        /// </summary>
        /// <param name="model">LoginViewModel containing username and password</param>
        /// <param name="ct">CancellationToken to allow request cancellation</param>
        /// <returns>Redirect to Dashboard on success, otherwise the login view with errors</returns>

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var employee = await _loginService.ValidateLoginAsync(model.Username, model.Password, ct);
            if (employee == null)
            {
                model.ErrorMessage = "Ongeldige gebruikersnaam of wachtwoord.";
                return View(model);
            }
            HttpContext.Session.SetObject("currentEmployee", model);
            return RedirectToAction("Index", "Dashboard");
        }
    }
}
