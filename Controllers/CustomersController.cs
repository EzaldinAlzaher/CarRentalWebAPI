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
    public class CustomersController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;

        public CustomersController(AppDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // - Create Customer
        [HttpPost]
        [Authorize]
        public ActionResult<Customer> CreateCustomer(CustomerDto model)
        {
            // Mapping between customer <-> customerForCreate
            var newCustomer = mapper.Map<Customer>(model);

            context.Customers.Add(newCustomer);
            context.SaveChanges();

            return CreatedAtRoute(
                "GetCustomer", new { customerId = newCustomer.Id }, newCustomer);
        }

        // Get Customers
        [HttpGet]
        public ActionResult GetCustomers()
        {
            var customers = context.Customers.ToList();

            if (!customers.Any())
                return NotFound("Customers not found");

            return Ok(customers);
        }

        // Get Customer 
        [HttpGet("{customerId}", Name = "GetCustomer")]
        public ActionResult GetCustomer(int customerId)
        {
            var customer = context.Customers.FirstOrDefault(c => c.Id == customerId);

            if (customer == null)
                return NotFound($"Customer {customerId} not found");

            return Ok(customer);
        }

        // - Update Customer
        [Authorize]
        [HttpPut("{customerId}")]
        public ActionResult<Customer> UpdateCustomer(
            int customerId,
            CustomerDto model)
        {
            var customer = context.Customers.FirstOrDefault(c => c.Id == customerId);

            if (customer == null)
                return NotFound();

            mapper.Map(model, customer);

            context.SaveChanges();

            return Ok(customer);
        }

        // - Delete Customer
        [Authorize]
        [HttpDelete("{customerId}")]
        public ActionResult DeleteCustomer(int customerId)
        {
            var customer = context.Customers.FirstOrDefault(c => c.Id == customerId);

            if (customer == null)
                return NotFound();

            if (context.Rentals.Any(r => r.CustomerId == customerId))
            {
                return Conflict("Cannot delete customer because it has related rentals.");
            }

            context.Customers.Remove(customer);
            context.SaveChanges();

            return NoContent();
        }

    }
}
