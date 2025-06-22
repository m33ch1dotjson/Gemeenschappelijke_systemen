using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IReservationRepository
    {
        Task<List<Reservation>> GetPendingReservationsAsync(CancellationToken ct);
        Task<Reservation?> GetByIdAsync(int reservationId, CancellationToken ct = default);

    }
}
