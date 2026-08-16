using AutoMapper;
using CarRental_WebAPI.DbContexts;
using CarRental_WebAPI.Models.DTOs;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRental_WebAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;

        public RentalsController(AppDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        // Create Rental
        [HttpPost]
        [Authorize]
        public ActionResult CreateRental(RentalDto model)
        {
            // Check Customer
            if (!context.Customers.Any(c => c.Id == model.CustomerId))
                return BadRequest("Customer not found.");

            // Get Vehicle with vihicleType for calc dailyrate
            var vehicle = context.Vehicles
                .Include(v => v.VehicleType)
                .FirstOrDefault(v => v.Id == model.VehicleId);

            if (vehicle == null)
                return BadRequest("Vehicle not found.");

            // Check Vehicle availability
            if (vehicle.Status != Status.Available)
                return Conflict("The vehicle is not available for rental.");

            // Validate dates
            if (model.EndDate <= model.StartDate)
                return BadRequest("End date must be after start date.");

            // Calculate number of days
            var numberOfDays = (model.EndDate.Date - model.StartDate.Date).Days;

            if (numberOfDays <= 0)
                return BadRequest("Rental period must be at least one day.");

            // Calculate total price
            var totalPrice = vehicle.VehicleType.DailyRate * numberOfDays;

            var newRental = mapper.Map<Rental>(model);

            newRental.TotalPrice = totalPrice;

            // Change vehicle status
            vehicle.Status = Status.Rented;

            context.Rentals.Add(newRental);
            context.SaveChanges();

            var rentalDto = mapper.Map<RentalShowDto>(newRental);

            return CreatedAtRoute(
                "GetRental",
                new { rentalId = newRental.Id },
                rentalDto);
        }


        // Get Rentals
        [HttpGet]
        public ActionResult GetRentals()
        {
            var rentals = context.Rentals.ToList();

            if (!rentals.Any())
                return NotFound("Rentals not found.");

            var rentalDtos = mapper.Map<List<RentalShowDto>>(rentals);

            return Ok(rentalDtos);
        }


        // Get Rental
        [HttpGet("{rentalId}", Name = "GetRental")]
        public ActionResult GetRental(int rentalId)
        {
            var rental = context.Rentals.FirstOrDefault(r => r.Id == rentalId);

            if (rental == null)
                return NotFound($"Rental {rentalId} not found.");

            var rentalDto = mapper.Map<RentalShowDto>(rental);

            return Ok(rentalDto);
        }


        // Get Customer Rentals
        // api/v1/Rentals/customer/{customerId}
        [HttpGet("customer/{customerId}")]
        public ActionResult GetCustomerRentals(int customerId)
        {
            // Check Customer
            if (!context.Customers.Any(c => c.Id == customerId))
                return NotFound($"Customer {customerId} not found.");

            var rentals = context.Rentals
                .Include(r => r.Vehicle)
                .Where(r => r.CustomerId == customerId)
                .ToList();

            if (!rentals.Any())
                return NotFound($"No rentals found for Customer {customerId}.");

            var rentalDtos = mapper.Map<List<RentalShowDto>>(rentals);

            return Ok(rentalDtos);
        }


        // Get Rentals By Date Range
        // api/v1/rentals/dateRange?startDate=2026-08-01&endDate=2026-08-25
        [HttpGet("dateRange")]
        public ActionResult GetRentalsByDateRange(DateTime startDate,DateTime endDate)
        {
            // Validate dates
            if (endDate <= startDate)
                return BadRequest("End date must be after start date.");

            var rentals = context.Rentals
                .Include(r => r.Vehicle)
                .Include(r => r.Customer)
                .Where(r =>
                    r.StartDate <= endDate &&
                    r.EndDate >= startDate)
                .ToList();

            if (!rentals.Any())
                return NotFound("No rentals found in the specified date range.");

            var rentalDtos = mapper.Map<List<RentalShowDto>>(rentals);

            return Ok(rentalDtos);
        }


        // Return Vehicle - status to Available
        [HttpPut("{rentalId}/return")]
        [Authorize]
        public IActionResult ReturnRental(int rentalId)
        {
            var rental = context.Rentals.FirstOrDefault(r => r.Id == rentalId);

            if (rental == null)
                return NotFound($"Rental {rentalId} not found.");

            var vehicle = context.Vehicles.FirstOrDefault(v => v.Id == rental.VehicleId);

            if (vehicle == null)
                return NotFound("Vehicle not found.");

            if (vehicle.Status != Status.Rented)
                return Conflict("This vehicle is not currently rented.");

            // switch status to available

            vehicle.Status = Status.Available;

            context.SaveChanges();

            return Ok("Rental returned successfully.");
        }
    }
}
