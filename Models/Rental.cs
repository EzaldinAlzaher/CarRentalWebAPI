using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Rental
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double TotalPrice { get; set; }

        // Vehicle 1:M Rental
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        // Rental 1:M Payment
        public List<Payment> Payments { get; set; } = new List<Payment>();

        // Customer 1:M Rental
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

    }
}
