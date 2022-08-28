using System;
using System.ComponentModel.DataAnnotations;

namespace WeightTracking.WebApi.ViewModels
{
    public class RegisterVM
    {
        [Required]
        [MinLength(5)]
        [MaxLength(64)]
        public string UserName { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(64)]
        public string Password { get; set; }
    }
}

