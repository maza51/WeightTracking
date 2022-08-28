using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using WeightTracking.DataAccess.Entities;

namespace WeightTracking.WebApi.ViewModels
{
    public class CreatePersonVM
    {
        [Required]
        [MinLength(5)]
        [MaxLength(64)]
        public string Name { get; set; }

        [Required]
        public bool IsPublic { get; set; }
    }
}

