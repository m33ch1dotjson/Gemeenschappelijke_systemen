using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Guest
    {
        private int _id;
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _email = string.Empty;
        private string _phoneNumber = string.Empty;

        public int GetId() => _id;
        public void SetId(int id) => _id = id;

        public string GetFirstName() => _firstName;
        public void SetFirstName(string firstName) => _firstName = firstName;

        public string GetLastName() => _lastName;
        public void SetLastName(string lastName) => _lastName = lastName;

        public string GetEmail() => _email;
        public void SetEmail(string email) => _email = email;

        public string GetPhoneNumber() => _phoneNumber;
        public void SetPhoneNumber(string phoneNumber) => _phoneNumber = phoneNumber;
    }
}

