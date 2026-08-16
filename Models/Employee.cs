using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Domain
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public DateTime HiredDate { get; set; }

        // readonly prop
        // Branch 1:M Employee
        public int BranchId { get; set; }
        public Branch Branch { get; set; }

    }
}
