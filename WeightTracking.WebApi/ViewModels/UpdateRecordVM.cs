using System;
using System.ComponentModel.DataAnnotations;

namespace WeightTracking.WebApi.ViewModels
{
    public class UpdateRecordVM
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [Range(0.1, 300)]
        public float Weigth { get; set; }

        [Required]
        [Range(0.1, 100)]
        public float Height { get; set; }
    }
}

