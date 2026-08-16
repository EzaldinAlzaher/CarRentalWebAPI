using Domain;

namespace CarRental_WebAPI.Models.DTOs
{
    public class VehicleDto
    {
        public int PlateNumber { get; set; }
        public string Model { get; set; }
        public int ManufactureYear { get; set; }
        public Status Status { get; set; }
        public int BranchId { get; set; }
        public int VehicleTypeId { get; set; }
    }
}
