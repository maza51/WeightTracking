using System;
using AutoMapper;
using WeightTracking.Application.DTOs;
using WeightTracking.DataAccess.Entities;

namespace WeightTracking.Application.Profiles
{
    public class RecordProfile : Profile
    {
        public RecordProfile()
        {
            CreateMap<Record, RecordDTO>().ReverseMap();
        }
    }
}

