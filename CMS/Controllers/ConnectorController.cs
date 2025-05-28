using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;

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
            var reservations = await _reservationRepository.GetPendingReservationsAsync(ct);
            return View(reservations);
        }
    }
}
