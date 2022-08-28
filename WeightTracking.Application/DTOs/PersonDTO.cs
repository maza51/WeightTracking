using System;
using WeightTracking.DataAccess.Entities;

namespace WeightTracking.Application.DTOs
{
    public class PersonDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsPublic { get; set; }
    }
}

