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
    public class VehicleTypesController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;

        public VehicleTypesController(AppDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // Create VehicleType
        [HttpPost]
        [Authorize]
        public ActionResult CreateVehicleType(VehicleTypeDto model)
        {
            if (context.VehicleTypes.Any(vt => vt.TypeName == model.TypeName))
                return Conflict("Vehicle type with this name already exists.");

            var newVehicleType = mapper.Map<VehicleType>(model);

            context.VehicleTypes.Add(newVehicleType);
            context.SaveChanges();

            return CreatedAtRoute(
                "GetVehicleType",
                new { vehicleTypeId = newVehicleType.Id },
                newVehicleType);
        }


        // Get VehicleTypes
        [HttpGet]
        public ActionResult GetVehicleTypes()
        {
            var vehicleTypes = context.VehicleTypes.ToList();

            if (!vehicleTypes.Any())
                return NotFound("Vehicle types not found.");

            return Ok(vehicleTypes);
        }


        // Get VehicleType
        [HttpGet("{vehicleTypeId}", Name = "GetVehicleType")]
        public ActionResult GetVehicleType(int vehicleTypeId)
        {
            var vehicleType = context.VehicleTypes.FirstOrDefault(vt => vt.Id == vehicleTypeId);

            if (vehicleType == null)
                return NotFound($"Vehicle type {vehicleTypeId} not found.");

            return Ok(vehicleType);
        }


        // Update VehicleType
        [HttpPut("{vehicleTypeId}")]
        [Authorize]
        public ActionResult<VehicleType> UpdateVehicleType(int vehicleTypeId, VehicleTypeDto model)
        {
            var vehicleType = context.VehicleTypes.FirstOrDefault(vt => vt.Id == vehicleTypeId);

            if (vehicleType == null)
                return NotFound("Vehicle type not found.");

            if (context.VehicleTypes.Any(vt => vt.TypeName == model.TypeName && vt.Id != vehicleTypeId))
                return Conflict("Another vehicle type with this name already exists.");

            mapper.Map(model, vehicleType);

            context.SaveChanges();

            return Ok(vehicleType);
        }


        // Delete VehicleType
        [HttpDelete("{vehicleTypeId}")]
        [Authorize]
        public ActionResult DeleteVehicleType(int vehicleTypeId)
        {
            var vehicleType = context.VehicleTypes.FirstOrDefault(vt => vt.Id == vehicleTypeId);

            if (vehicleType == null)
                return NotFound("Vehicle type not found.");

            if (context.Vehicles.Any(v => v.VehicleTypeId == vehicleTypeId))
                return Conflict("Cannot delete vehicle type because it has related vehicles");

            context.VehicleTypes.Remove(vehicleType);
            context.SaveChanges();

            return NoContent();
        }


    }
}
