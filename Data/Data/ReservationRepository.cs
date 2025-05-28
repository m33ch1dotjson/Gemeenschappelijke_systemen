using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using System.Data;
using MySqlConnector;

namespace Infrastructure.Data
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly MySqlConnection _connection;

        public ReservationRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<Reservation>> GetPendingReservationsAsync(CancellationToken ct = default)
        {
            if (_connection.State != System.Data.ConnectionState.Open)
                await _connection.OpenAsync(ct);

            const string sql = @"
        SELECT 
            g.FirstName, g.LastName, g.Email, g.PhoneNumber,
            r.TotalPrice, r.CheckInDate, r.CheckOutDate
        FROM Reservation r
        LEFT JOIN Guest g ON g.Id = r.GuestId
        WHERE r.InvoiceSent = false";

            using var command = new MySqlCommand(sql, _connection);
            using var reader = await command.ExecuteReaderAsync(ct);

            var reservations = new List<Reservation>();

            while (await reader.ReadAsync(ct))
            {
                var reservation = new Reservation();
                var guest = new Guest();

                reservation.SetTotalPrice(reader.GetDecimal(reader.GetOrdinal("TotalPrice")));
                reservation.SetCheckInDate (reader.GetDateTime(reader.GetOrdinal("CheckInDate")));
                reservation.SetCheckOutDate(reader.GetDateTime(reader.GetOrdinal("CheckOutDate")));
                guest.SetFirstName(reader.GetString(reader.GetOrdinal("FirstName")));
                guest.SetLastName(reader.GetString(reader.GetOrdinal("LastName")));
                guest.SetEmail(reader.GetString(reader.GetOrdinal("Email")));
                guest.SetPhoneNumber(reader.GetString(reader.GetOrdinal("PhoneNumber")));

                reservation.SetGuest(guest);

                reservations.Add(reservation);
            }

            return reservations;
        }

    }
}
