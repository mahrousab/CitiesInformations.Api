﻿namespace CitiesInformations.Api.Models
{
    public class CityDto
    {

        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int NumberOfPointsOfInterest
        {
            get
            {

                return PointOfInterest.Count;
            }
        }

        public ICollection<PointOfInterestDto> PointOfInterest { get; set; }

        = new List<PointOfInterestDto>();


    }
}
