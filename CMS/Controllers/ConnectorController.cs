using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using CMS.Models;
using CMS.ViewModels;
using Application.Services;

namespace CMS.Controllers
{
    public class ConnectorController : Controller
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IRompslompService _rompslompService;



        public ConnectorController(IReservationRepository reservationRepository, IRompslompService rompslompService)
        {
            _reservationRepository = reservationRepository;
            _rompslompService = rompslompService;
        }

        /// <summary>
        /// Shows all pending reservations that have not yet been sent to Rompslomp.
        /// </summary>
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

        /// <summary>
        /// Sends a specific reservation to Rompslomp as an invoice.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> SendToRompslomp(int reservationId, CancellationToken ct)
        {
            // Get the reservation by its ID
            var reservation = await _reservationRepository.GetByIdAsync(reservationId, ct);
            if (reservation == null)
            {
                TempData["Error"] = $"Reservation with ID {reservationId} not found.";
                return RedirectToAction("Index");
            }

            try
            {
                // Send the reservation to Rompslomp and get the resulting invoice ID
                var invoiceId = await _rompslompService.SendReservationAsInvoiceAsync(reservation, ct);

                // Show success message
                TempData["Success"] = $"Invoice created successfully in Rompslomp (Invoice ID: {invoiceId}).";
            }
            catch (Exception ex)
            {
                // Show error message on failure
                TempData["Error"] = $"Error while sending reservation: {ex.Message}";
            }

            // Redirect back to the overview
            return RedirectToAction("Index");
        }


    }
}
