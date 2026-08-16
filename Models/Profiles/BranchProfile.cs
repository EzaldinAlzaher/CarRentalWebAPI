using AutoMapper;
using CarRental_WebAPI.Models.DTOs;
using Domain;

namespace CarRental_WebAPI.Models.Profiles
{
    public class BranchProfile : Profile
    {
        public BranchProfile()
        {
            CreateMap<Branch, BranchDto>().ReverseMap();
        }
    }
}
