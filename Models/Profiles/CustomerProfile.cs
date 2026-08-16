using AutoMapper;
using CarRental_WebAPI.Models.DTOs;
using Domain;

namespace CarRental_WebAPI.Models.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerDto>().ReverseMap();
        }
    }
}
