using AutoMapper;
using CarRental_WebAPI.DbContexts;
using CarRental_WebAPI.Models.DTOs;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRental_WebAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;

        public VehiclesController(AppDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // Create Vehicle
        [HttpPost]
        [Authorize]
        public ActionResult CreateVehicle(VehicleDto model)
        {
            // Check if plate number already exists
            if (context.Vehicles.Any(v => v.PlateNumber == model.PlateNumber))
                return Conflict("Vehicle with this plate number already exists");

            // Check if branch exists
            if (!context.Branches.Any(b => b.Id == model.BranchId))
                return BadRequest("Branch not found");

            // Check if vehicle type exists
            if (!context.VehicleTypes.Any(vt => vt.Id == model.VehicleTypeId))
                return BadRequest("Vehicle type not found");

            var newVehicle = mapper.Map<Vehicle>(model);

            context.Vehicles.Add(newVehicle);
            context.SaveChanges();

            return CreatedAtRoute(
                "GetVehicle",
                new { vehicleId = newVehicle.Id },
                newVehicle);
        }


        // Get Vehicles
        [HttpGet]
        public ActionResult GetVehicles()
        {
            var vehicles = context.Vehicles.ToList();

            if (!vehicles.Any())
                return NotFound("Vehicles not found.");

            return Ok(vehicles);
        }


        // Get Vehicle
        [HttpGet("{vehicleId}", Name = "GetVehicle")]
        public ActionResult GetVehicle(int vehicleId)
        {
            var vehicle = context.Vehicles.FirstOrDefault(v => v.Id == vehicleId);

            if (vehicle == null)
                return NotFound($"Vehicle {vehicleId} not found.");

            return Ok(vehicle);
        }


        // Update Vehicle
        [HttpPut("{vehicleId}")]
        [Authorize]
        public ActionResult<Vehicle> UpdateVehicle(int vehicleId, VehicleDto model)
        {
            var vehicle = context.Vehicles.FirstOrDefault(v => v.Id == vehicleId);

            if (vehicle == null)
                return NotFound("Vehicle not found.");

            // Check duplicate plate number
            if (context.Vehicles.Any(v => v.PlateNumber == model.PlateNumber && v.Id != vehicleId))
                return Conflict("Another vehicle with this plate number already exists.");

            // Check if branch exists
            if (!context.Branches.Any(b => b.Id == model.BranchId))
                return BadRequest("Branch not found.");

            // Check if vehicle type exists
            if (!context.VehicleTypes.Any(vt => vt.Id == model.VehicleTypeId))
                return BadRequest("Vehicle type not found.");

            mapper.Map(model, vehicle);

            context.SaveChanges();

            return Ok(vehicle);
        }


        // Delete Vehicle
        [HttpDelete("{vehicleId}")]
        [Authorize]
        public ActionResult DeleteVehicle(int vehicleId)
        {
            var vehicle = context.Vehicles.FirstOrDefault(v => v.Id == vehicleId);

            if (vehicle == null)
                return NotFound("Vehicle not found");

            // Cannot delete vehicle if it has rentals
            if (context.Rentals.Any(r => r.VehicleId == vehicleId))
                return Conflict("Cannot delete vehicle because it has related rentals");

            context.Vehicles.Remove(vehicle);
            context.SaveChanges();

            return NoContent();
        }


    }
}
