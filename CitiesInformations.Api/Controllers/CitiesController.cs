using Asp.Versioning;
using AutoMapper;
using CitiesInformations.Api.Models;
using CitiesInformations.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text.Json;

namespace CitiesInformations.Api.Controllers
{

    [ApiController]
    [Route("api/Cities")]

    [ApiVersion(1)]
    [ApiVersion(2)]
  //  [Authorize]
    
    public class CitiesController : ControllerBase
    {

        private readonly ICityInfoRepository _Repo;
        private readonly IMapper _mapper;

        const int maxCitiesPageSize = 20;

        public CitiesController(ICityInfoRepository Repo,
            IMapper mapper)
        {
            _Repo = Repo?? throw new Exception(nameof(Repo)); ;

            _mapper = mapper ?? throw new Exception(nameof(mapper));

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities(
            string? name,
            string? searchQuery, int pageNumber = 1, int pageSize= 10)
        {

            if (pageSize > maxCitiesPageSize)
            {
                pageSize = maxCitiesPageSize;
            }

            var (cityEntities, paginationMetadata) = await _Repo.GetCitiesAsync(name, searchQuery, pageNumber, pageSize);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));


                
            return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
            
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        
        public async Task<IActionResult> GetId(int id, bool includePointsOfInterest = false)
        {




            var city = await _Repo.GetCityAsync(id, includePointsOfInterest);

            if(city == null)
            {
                return NotFound();
            }

            if (includePointsOfInterest)
            {
                return Ok(_mapper.Map<CityDto>(city));
            }
            return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
        }

    }
}
