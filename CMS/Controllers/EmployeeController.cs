using CMS.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using CMS.Models;

namespace CMS.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeController(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var LoginViewModel = HttpContext.Session.GetObject<LoginViewModel>("currentEmployee");
            if (LoginViewModel == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var medewerkers = await _repository.GetAllAsync(ct);


            return View(medewerkers); 
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View(new EmployeeCreateViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeCreateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            var medewerker = new Employee();

            try
            {
                medewerker.SetUsername(model.Username);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(nameof(model.Username), ex.Message);
            }

            try
            {
                medewerker.SetFullName(model.FullName);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(nameof(model.FullName), ex.Message);
            }

            try
            {
                medewerker.SetPassword(model.Password);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(nameof(model.Password), ex.Message);
            }

            if (!ModelState.IsValid)
                return View(model);

            await _repository.AddAsync(medewerker, ct);
            return RedirectToAction("Index");
        }
    }
}
