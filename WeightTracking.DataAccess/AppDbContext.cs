using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WeightTracking.DataAccess.Entities;

namespace WeightTracking.DataAccess
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Record> Records { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Person>(p =>
            {
                p.HasKey(x => x.Id);
                p.Property(x => x.Name).IsRequired();
                p.Property(x => x.IsPublic).HasDefaultValue(false);
                p.Property(x => x.OwnerName).IsRequired();
            });

            modelBuilder.Entity<Record>(p =>
            {
                p.HasKey(x => x.Id);
                p.Property(x => x.Date).IsRequired();
                p.Property(x => x.Height).IsRequired();
                p.Property(x => x.Weigth).IsRequired();

                p.HasOne<Person>(x => x.Person)
                    .WithMany(x => x.Records)
                    .HasForeignKey(x => x.PersonId);
            });
        }
    }
}

