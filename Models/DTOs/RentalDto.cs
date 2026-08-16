namespace CarRental_WebAPI.Models.DTOs
{
    public class RentalDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CustomerId { get; set; }
        public int VehicleId { get; set; }
    }
}
