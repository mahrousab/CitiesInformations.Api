using CitiesInformations.Api.Entities;
using CitiesInformations.Api.Services;

namespace CitiesInformations.Api.Repositories
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(string? name,
            string? searchQuery, int pageNumber, int pageSize);

        Task<City> GetCityAsync(int cityId, bool includePointsOfInterest);


        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestsForCityAsync(int cityId);

        Task<bool> CityExistsAsync(int cityId);
        Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId,
            int pointOfInterestId);

        Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);

        Task<bool> CityNameMatchesCityId(string? cityName, int cityId);
        Task<bool> SaveChangesAsync();


        void DeletePointOfInterestForCityAsync(PointOfInterest pointOfInterest);
    }
}
