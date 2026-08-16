using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Branch
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        // Branch 1:M Employee
        public List<Employee> Employees { get; set; } = new List<Employee>();

        // Branch 1:M Vehicle

        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

    }
}
