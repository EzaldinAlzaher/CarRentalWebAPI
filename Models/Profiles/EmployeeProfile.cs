using AutoMapper;
using CarRental_WebAPI.Models.DTOs;
using Domain;

namespace CarRental_WebAPI.Models.Profiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDto>().ReverseMap();
        }
    }
}
