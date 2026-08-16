using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.WebRequestMethods;

namespace Domain
{
    public class VehicleType
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public int SeatsCount { get; set; }
        public int DailyRate { get; set; }

        // VehicleType 1:M Vehicle
        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

    }
}
