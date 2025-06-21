using CMS.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using CMS.Models;
using Domain.Comparers;

namespace CMS.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeController(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Shows a list of employees, sorted by name A-Z or Z-A depending on the query.
        /// Requires a valid session (i.e., logged-in user).
        /// </summary>
        /// <param name="sortOrder">The desired sort direction: "asc" or "desc"</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The sorted employee list view</returns>
        public async Task<IActionResult> Index(string sortOrder = "asc", CancellationToken ct = default)
        {
            // Session check: redirect to login if no active user session
            var LoginViewModel = HttpContext.Session.GetObject<LoginViewModel>("currentEmployee");
            if (LoginViewModel == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var medewerkers = await _repository.GetAllAsync(ct);

            // Use the custom comparer to sort by full name
            var comparer = new EmployeeComparer(descending: sortOrder == "desc");
            medewerkers.Sort(comparer);

            // Store the reverse order for toggling in the view button
            ViewBag.SortOrder = sortOrder == "desc" ? "asc" : "desc";

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
