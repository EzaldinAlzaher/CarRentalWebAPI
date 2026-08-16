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
    public class BranchesController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;

        public BranchesController(AppDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        // Create Branch
        [HttpPost]
        [Authorize]
        public ActionResult CreateBranch(BranchDto model)
        {
            if (context.Branches.Any(b => b.City == model.City))
                return Conflict("A branch already exists in this city.");

            var newBranch = mapper.Map<Branch>(model);

            context.Branches.Add(newBranch);
            context.SaveChanges();

            return CreatedAtRoute(
                "GetBranch",
                new { branchId = newBranch.Id },
                newBranch);
        }

        // Get Branches
        [HttpGet]
        public ActionResult GetBranches()
        {
            var branches = context.Branches.ToList();

            if (!branches.Any())
                return NotFound("Branches not found.");

            return Ok(branches);
        }

        // Get Branch
        [HttpGet("{branchId}", Name = "GetBranch")]
        public ActionResult GetBranch(int branchId)
        {
            var branch = context.Branches.FirstOrDefault(b => b.Id == branchId);

            if (branch == null)
                return NotFound($"Branch {branchId} not found.");

            return Ok(branch);
        }

        // Update Branch
        [Authorize]
        [HttpPut("{branchId}")]
        public ActionResult<Branch> UpdateBranch(int branchId, BranchDto model)
        {
            var branch = context.Branches.FirstOrDefault(b => b.Id == branchId);

            if (branch == null)
                return NotFound("Branch not found.");

            if (context.Branches.Any(b => b.City == model.City && b.Id != branchId))
                return Conflict("A branch already exists in this city.");


            mapper.Map(model, branch);

            context.SaveChanges();

            return Ok(branch);
        }

        // Delete Branch
        [Authorize]
        [HttpDelete("{branchId}")]
        public ActionResult DeleteBranch(int branchId)
        {
            var branch = context.Branches.FirstOrDefault(b => b.Id == branchId);

            if (branch == null)
                return NotFound("Branch not found.");

            bool hasEmployees = context.Employees.Any(e => e.BranchId == branchId);

            bool hasVehicles = context.Vehicles.Any(v => v.BranchId == branchId);

            if (hasEmployees || hasVehicles)
                return Conflict("Cannot delete branch because it has related employees or vehicles.");

            context.Branches.Remove(branch);
            context.SaveChanges();

            return NoContent();
        }

    }
}
