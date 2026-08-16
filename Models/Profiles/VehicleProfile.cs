using AutoMapper;
using CarRental_WebAPI.Models.DTOs;
using Domain;

namespace CarRental_WebAPI.Models.Profiles
{
    public class VehicleProfile : Profile
    {
        public VehicleProfile()
        {
            CreateMap<Vehicle, VehicleDto>().ReverseMap();
        }
    }
}
