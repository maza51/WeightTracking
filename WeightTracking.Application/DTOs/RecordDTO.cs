using System.ComponentModel.DataAnnotations;
using WeightTracking.DataAccess.Entities;

namespace WeightTracking.Application.DTOs
{
    public class RecordDTO
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public float Weigth { get; set; }

        public float Height { get; set; }
    }
}