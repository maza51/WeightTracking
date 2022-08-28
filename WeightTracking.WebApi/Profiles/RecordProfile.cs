using System;
using AutoMapper;
using WeightTracking.Application.DTOs;
using WeightTracking.DataAccess.Entities;
using WeightTracking.WebApi.ViewModels;

namespace WeightTracking.WebApi.Profiles
{
    public class RecordProfile : Profile
    {
        public RecordProfile()
        {
            CreateMap<CreateRecordVM, RecordDTO>().ReverseMap();

            CreateMap<UpdateRecordVM, RecordDTO>().ReverseMap();
        }
    }
}

