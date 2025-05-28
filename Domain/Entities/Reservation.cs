using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Reservation
    {
        private decimal _totalPrice;
        private Guest? _guest;
        private DateTime _checkInDate;
        private DateTime _checkOutDate;

        public decimal GetTotalPrice() => _totalPrice;
        public void SetTotalPrice(decimal totalPrice) => _totalPrice = totalPrice;

        public Guest GetGuest() => _guest;

        public void SetGuest(Guest guest)
        {
            _guest = guest;
        }

        public DateTime GetCheckInDate() => _checkInDate;

        public void SetCheckInDate(DateTime date)
        {
            _checkInDate = date;
        }

        public DateTime GetCheckOutDate() => _checkOutDate;

        public void SetCheckOutDate(DateTime date)
        {;
            _checkOutDate = date;
        }
    }
}
