using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using CMS.Models;
using CMS.ViewModels;

namespace CMS.Controllers
{
    public class ConnectorController : Controller
    {
        private readonly IReservationRepository _reservationRepository;

        public ConnectorController(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            // Only accessible with a valid session token 
            var LoginViewModel = HttpContext.Session.GetObject<LoginViewModel>("currentEmployee");
            if (LoginViewModel == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var reservations = await _reservationRepository.GetPendingReservationsAsync(ct);
            return View(reservations);
        }
    }
}
