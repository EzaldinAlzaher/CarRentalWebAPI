using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace Domain
{
    public class Vehicle
    {
        public int Id { get; set; }
        public int PlateNumber { get; set; }
        public string Model { get; set; }
        public int ManufactureYear { get; set; }
        public Status Status { get; set; }

        // Branch 1:M Vehicle
        public int BranchId { get; set; }
        public Branch Branch { get; set; }

        // VehicleType 1:M Vehicle
        public int VehicleTypeId { get; set; }
        public VehicleType VehicleType { get; set; }

        // Vehicle 1:M Rental
        public List<Rental> Rentals { get; set; } = new List<Rental>();

    }

    public enum Status
    {
        Available,
        Rented,
        Maintenance,
        OutOfService
    }

}
