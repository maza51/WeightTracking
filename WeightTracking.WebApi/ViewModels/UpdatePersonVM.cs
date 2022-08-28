using System;
using System.ComponentModel.DataAnnotations;

namespace WeightTracking.WebApi.ViewModels
{
    public class UpdatePersonVM
    {
        [Required]
        [MinLength(5)]
        [MaxLength(64)]
        public string Name { get; set; }

        [Required]
        public bool IsPublic { get; set; }
    }
}

