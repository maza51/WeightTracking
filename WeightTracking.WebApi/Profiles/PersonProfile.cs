using System;
using AutoMapper;
using WeightTracking.Application.DTOs;
using WeightTracking.DataAccess.Entities;
using WeightTracking.WebApi.ViewModels;

namespace WeightTracking.WebApi.Profiles
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<CreatePersonVM, PersonDTO>().ReverseMap();

            CreateMap<UpdatePersonVM, PersonDTO>().ReverseMap();
        }
    }
}

