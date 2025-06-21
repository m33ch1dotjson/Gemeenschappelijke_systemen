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
