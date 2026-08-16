using AutoMapper;
using CarRental_WebAPI.Models.DTOs;
using Domain;

namespace CarRental_WebAPI.Models.Profiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {

            CreateMap<Payment, PaymentDto>().ReverseMap();
        }
    }
}
