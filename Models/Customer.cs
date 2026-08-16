using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Domain
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int LicenseNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        // Customer 1:M Rental
        public List<Rental> Rentals { get; set; } = new List<Rental>();

    }
}
