using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Payment
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public DateTime CreatedDate { get; set; }

        // Rental 1:M Payment
        public int RentalId { get; set; }
        public Rental Rental { get; set; }

    }
}
