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
    public class PaymentsController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;

        public PaymentsController(AppDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        // Create Payment
        [HttpPost]
        [Authorize]
        public ActionResult CreatePayment(PaymentDto model)
        {
            // Check Rental
            var rental = context.Rentals.FirstOrDefault(r => r.Id == model.RentalId);

            if (rental == null)
                return BadRequest("Rental not found.");

            // Validate Amount
            if (model.Amount <= 0)
                return BadRequest("Payment amount must be greater than zero.");

            // Create Payment
            var payment = mapper.Map<Payment>(model);

            payment.CreatedDate = DateTime.UtcNow;

            context.Payments.Add(payment);
            context.SaveChanges();

            return CreatedAtRoute(
                "GetPayment",
                new { paymentId = payment.Id },
                payment);
        }


        // Get Payments
        [HttpGet]
        public ActionResult GetPayments()
        {
            var payments = context.Payments.ToList();

            if (!payments.Any())
                return NotFound("Payments not found.");

            return Ok(payments);
        }


        // Get Payment
        [HttpGet("{paymentId}", Name = "GetPayment")]
        public ActionResult GetPayment(int paymentId)
        {
            var payment = context.Payments.FirstOrDefault(p => p.Id == paymentId);

            if (payment == null)
                return NotFound($"Payment {paymentId} not found.");

            return Ok(payment);
        }


        // Get Payments By Rental
        [HttpGet("rental/{rentalId}")]
        public ActionResult GetPaymentsByRental(int rentalId)
        {
            // Check Rental
            if (!context.Rentals.Any(r => r.Id == rentalId))
                return NotFound($"Rental {rentalId} not found.");

            var payments = context.Payments.Where(p => p.RentalId == rentalId).ToList();

            if (!payments.Any())
                return NotFound($"No payments found for Rental {rentalId}.");

            return Ok(payments);
        }


        // Get Company Revenue By Date Range
        [HttpGet("revenue")]
        [Authorize]
        public ActionResult GetRevenue(DateTime startDate, DateTime endDate)
        {
            // Validate dates
            if (endDate <= startDate)
                return BadRequest("End date must be after start date.");

            var revenue = context.Payments
                .Where(p =>
                    p.CreatedDate >= startDate &&
                    p.CreatedDate <= endDate)
                .Sum(p => p.Amount);

            return Ok($"Revenue is {revenue}");
        }

    }
}
