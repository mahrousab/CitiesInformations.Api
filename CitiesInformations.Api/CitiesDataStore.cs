using CitiesInformations.Api.Models;

namespace CitiesInformations.Api
{
    public class CitiesDataStore
    {

        public List<CityDto> Cities { get; set; }

        public static CitiesDataStore Current { get; } = new CitiesDataStore();
        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {

                    Id = 1,
                    Name = "New York City",
                    Description= "the one of the big park",


                    PointOfInterest  = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                    {
                        Id=2,
                        Name = "ant",
                        Description ="bla bla"
                    }
                    }
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Antwerb",
                    Description = "the one with cathedral",
                    PointOfInterest = new List<PointOfInterestDto>
                    {
                        new PointOfInterestDto()
                    {
                        Id= 3,
                        Name = "Elllitht",
                        Description = "that is fine"
                    }

                    }
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "paris",
                    Description = "that is on with big tour",

                    PointOfInterest = new List<PointOfInterestDto>
                    {
                        new PointOfInterestDto()
                    {
                        Id=4,
                        Name = "NewPark",
                        Description = "my own group",
                    }
                    }

                }
            };
        }

    }
}
