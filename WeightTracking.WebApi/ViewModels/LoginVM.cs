using System;
using System.ComponentModel.DataAnnotations;

namespace WeightTracking.WebApi.ViewModels
{
    public class LoginVM
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

