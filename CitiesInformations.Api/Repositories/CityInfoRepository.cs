using CitiesInformations.Api.DbContexts;
using CitiesInformations.Api.Entities;
using CitiesInformations.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace CitiesInformations.Api.Repositories
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly ApplicationDbContext _context;
        public CityInfoRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _context.Cities.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<City> GetCityAsync(int cityId, bool includePointsOfInterest)
        {
            if(includePointsOfInterest)
            {
                return await _context.Cities.Include(x => x.PointOfInterests)
                    .Where(x => x.Id == cityId)
                    .FirstOrDefaultAsync();
            }

            return await _context.Cities.Where(x => x.Id == cityId).FirstOrDefaultAsync();
        }

        public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(
            int cityId,
            int pointOfInterestId)
        {
            return await _context.PointOfInterests
                 .Where(x => x.CityId == cityId && x.Id == pointOfInterestId)
                 .FirstOrDefaultAsync();
        }

        public async Task<bool> CityExistsAsync(int cityId)
        {
            return await _context.Cities.AnyAsync(x => x.Id == cityId);
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestsForCityAsync(int cityId)
        {
            return await _context.PointOfInterests
                .Where(x => x.CityId == cityId).ToListAsync();
        }

        public async Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest)
        {
            var city = await GetCityAsync(cityId, false);

            if(city!= null)
            {
                city.PointOfInterests.Add(pointOfInterest);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0); 
        }

        public void DeletePointOfInterestForCityAsync(PointOfInterest pointOfInterest)
        {
            _context.PointOfInterests.Remove(pointOfInterest);
        }

        public async Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize)
        {
            
            // collection to start from 

            var collection = _context.Cities as IQueryable<City>;

            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                collection = collection.Where(x => x.Name == name);
            }


            if(!string.IsNullOrWhiteSpace(name))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(x=>x.Name.Contains(searchQuery) || 
                (x.Description != null && x.Description.Contains(searchQuery)));
            }

            var totalItemCount = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await collection.OrderBy(x=>x.Name)
                .Skip(pageSize*(pageNumber-1)).
                Take(pageSize).
                ToListAsync();
            return (collectionToReturn, paginationMetadata);
        }

        public async Task<bool> CityNameMatchesCityId(string? cityName, int cityId)
        {
            return await _context.Cities.AnyAsync(x=>x.Id == cityId && x.Name == cityName);
        }
    }
}
