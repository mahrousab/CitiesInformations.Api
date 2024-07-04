using CitiesInformations.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace CitiesInformations.Api.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> PointOfInterests { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<City>()
                .HasData(new City("New York City")
                {
                    Id = 1,
                    Description = "It is that one with big park"
                },
                new City("Qairo")
                {
                    Id = 2,
                    Description = "I Like Qairo, I Love this city"
                },
                new City("paris")
                {
                    Id = 3,
                    Description = " Paris is good"

                });

            modelBuilder.Entity<PointOfInterest>().
                HasData(new PointOfInterest("Central Park")
                {
                    Id = 1,
                    CityId = 1,
                    Description = "gooooooood!, The Most Visited"
                },
                new PointOfInterest("Empire State")
                {
                    Id = 2,
                    CityId = 2,
                    Description = "very good I Love This Park"
                },

                new PointOfInterest("Cathedral")
                {
                    Id = 3,
                    CityId = 3,
                    Description = "Located In Manhattan"
                });


            base.OnModelCreating(modelBuilder);
        }

    }




}
