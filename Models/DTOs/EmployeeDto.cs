namespace CarRental_WebAPI.Models.DTOs
{
    public class EmployeeDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public DateTime HiredDate { get; set; }
        public int BranchId { get; set; }
    }
}
