using Asp.Versioning;
using AutoMapper;
using CitiesInformations.Api.Entities;
using CitiesInformations.Api.Models;
using CitiesInformations.Api.Repositories;
using CitiesInformations.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CitiesInformations.Api.Controllers
{

    [ApiController]
    [Route("api/Cities/{cityId}PointOfInterest")]
    //  [Authorize(Policy = "MustBeFromAntwerp")]

    [ApiVersion(2)]
    
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;

        private readonly ICityInfoRepository _Repo;
        private readonly IMapper _mapper;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService, ICityInfoRepository Repo, IMapper mapper)
        {
            _logger = logger ?? throw new Exception(nameof(logger));
            _mailService = mailService ?? throw new Exception(nameof(mailService));
            _Repo = Repo ?? throw new Exception(nameof(Repo));
            _mapper = mapper ?? throw new Exception(nameof(mapper));
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointOfInterest(
            int cityId)
        {

            var cityName = User.Claims.FirstOrDefault(x=>x.Type == "City")? .Value;

            if(!await _Repo.CityNameMatchesCityId(cityName, cityId))
            {

                return Forbid();
            }

            if(!await _Repo.CityExistsAsync(cityId))
            {
                _logger.LogInformation(
                    $"CityWithId {cityId} was not found"

                    );

                return NotFound();
            }

            var pointOfInterestForCity = await _Repo.GetPointsOfInterestsForCityAsync
                (cityId);

            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointOfInterestForCity));
        }

        [HttpGet("{pointofinterestid}"), ActionName("GetPointOfInterest")]

        public async Task <ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            if(!await _Repo.CityExistsAsync(cityId))
            {
                return NotFound();
            }
            var pointOfInterest= await _Repo
                .GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if(pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));
        }

        [HttpPost]

        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(int cityId, [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {

          if(!await _Repo.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            // Mapping 

            var finalPointOfInterest = _mapper.Map<Entities.PointOfInterest>(pointOfInterest);


            await _Repo.AddPointOfInterestForCityAsync(cityId, finalPointOfInterest);

            await _Repo.SaveChangesAsync();

            
            var createdPointForInterestToReturn =
                _mapper.Map < Models.PointOfInterestDto> (finalPointOfInterest);
            
            
            return CreatedAtRoute("GetPointInterest",
                new
                {
                    cityId = cityId,
                    pointOfInterestId = createdPointForInterestToReturn.Id
                },
                createdPointForInterestToReturn);
        }

        [HttpPut("{pointofinterestid}")]

        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId,

            PointOfInterestForUpdateDto pointOfInterest)
        {
            if(!await _Repo.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = await _Repo.
                GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
            if(pointOfInterestEntity == null)
            {
                return NotFound();
            }
            _mapper.Map(pointOfInterest,pointOfInterestEntity);


            await _Repo.SaveChangesAsync();

            
            return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]


        public async Task <ActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId)
        {

            if(!await _Repo.CityExistsAsync(cityId))
            {
                return NotFound();
            }
            var pointOfInterestEntity = await _Repo

                .GetPointOfInterestForCityAsync (cityId, pointOfInterestId);

            if( pointOfInterestEntity == null)
            {
                return NotFound();
            }

            _Repo.DeletePointOfInterestForCityAsync(pointOfInterestEntity);
            await _Repo.SaveChangesAsync();


            

            _mailService.Send("point of Interest deleted",

                $"point of interest{pointOfInterestEntity.Name} with id {pointOfInterestEntity.Id} was deleted");
            return NoContent();
        }
    }
}
