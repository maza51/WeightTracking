using System;
namespace WeightTracking.DataAccess.Entities
{
    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsPublic { get; set; }

        public ICollection<Record> Records { get; set; }

        public string OwnerName { get; set; }
    }
}

