using AutoMapper;
using CarRental_WebAPI.Models.DTOs;
using Domain;

namespace CarRental_WebAPI.Models.Profiles
{
    public class RentalProfile : Profile
    {
        public RentalProfile()
        {
            CreateMap<Rental, RentalDto>().ReverseMap();

            CreateMap<RentalShowDto, Rental>().ReverseMap();

        }
    }
}
