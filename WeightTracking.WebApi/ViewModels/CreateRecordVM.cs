using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using WeightTracking.DataAccess.Entities;

namespace WeightTracking.WebApi.ViewModels
{
    public class CreateRecordVM
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

