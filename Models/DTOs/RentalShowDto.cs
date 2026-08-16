namespace CarRental_WebAPI.Models.DTOs
{
    public class RentalShowDto
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public double TotalPrice { get; set; }

        public int CustomerId { get; set; }
        public int VehicleId { get; set; }
    }
}
