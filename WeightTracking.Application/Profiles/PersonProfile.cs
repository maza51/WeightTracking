using System;
using AutoMapper;
using WeightTracking.Application.DTOs;
using WeightTracking.DataAccess.Entities;

namespace WeightTracking.Application.Profiles
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<Person, PersonDTO>().ReverseMap();
        }
    }
}

