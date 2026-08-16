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
    public class EmployeesController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;

        public EmployeesController(AppDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        // Create Employee
        [HttpPost]
        [Authorize]
        public ActionResult CreateEmployee(EmployeeDto model)
        {
            // check if branch is exists
            if (!context.Branches.Any(b => b.Id == model.BranchId))
                return BadRequest("Branch not found!");

            var newEmployee = mapper.Map<Employee>(model);

            context.Employees.Add(newEmployee);

            context.SaveChanges();

            return CreatedAtRoute(
                "GetEmployee",
                new { employeeId = newEmployee.Id },
                newEmployee);
        }

        // Get Employees
        [HttpGet]
        public ActionResult GetEmployees()
        {
            var employees = context.Employees.ToList();

            if (!employees.Any())
                return NotFound("Employees not found");

            return Ok(employees);
        }

        // Get Employee
        [HttpGet("{employeeId}", Name = "GetEmployee")]
        public ActionResult GetEmployee(int employeeId)
        {
            var employee = context.Employees.FirstOrDefault(e => e.Id == employeeId);

            if (employee == null)
                return NotFound($"Employee {employeeId} not found");

            return Ok(employee);
        }

        // - Update Employee
        [HttpPut("{employeeId}")]
        [Authorize]
        public ActionResult<Employee> UpdateEmployee(int employeeId, EmployeeDto model)
        {
            var employee = context.Employees.FirstOrDefault(e => e.Id == employeeId);

            if (employee == null)
                return NotFound("Employee not found");

            if (!context.Branches.Any(b => b.Id == model.BranchId))
                return BadRequest("Branch not found.");

            mapper.Map(model, employee);

            context.SaveChanges();

            return Ok(employee);
        }

        // - Delete Employee
        [HttpDelete("{employeeId}")]
        [Authorize]
        public ActionResult DeleteEmployee(int employeeId)
        {
            var employee = context.Employees.FirstOrDefault(e => e.Id == employeeId);

            if (employee == null)
                return NotFound("Employee not found");

            context.Employees.Remove(employee);
            context.SaveChanges();

            return NoContent();
        }

    }
}
