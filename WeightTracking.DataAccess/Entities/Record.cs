using System;
using System.ComponentModel.DataAnnotations;

namespace WeightTracking.DataAccess.Entities
{
    public class Record
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public float Weigth { get; set; }

        public float Height { get; set; }

        public int PersonId { get; set; }

        public Person Person { get; set; }
    }
}

