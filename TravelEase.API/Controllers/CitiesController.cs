using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TravelEase.API.Common.Responses;
using TravelEase.Application.CityManagement.Commands;
using TravelEase.Application.CityManagement.DTOs.Requests;
using TravelEase.Application.CityManagement.DTOs.Responses;
using TravelEase.Application.CityManagement.Queries;

namespace TravelEase.API.Controllers
{
    [Route("api/cities")]
    [ApiController]
    [ApiVersion("1.0")]

    public class CitiesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CitiesController(IMediator mediator,IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllCitiesAsync([FromQuery] GetAllCitiesQuery cityQuery)
        {
            var paginatedListOfCities = await _mediator.Send(cityQuery);
            Response.Headers.Append("X-Pagination", 
                JsonSerializer.Serialize(paginatedListOfCities.PageData));


            if (!cityQuery.IncludeHotels)
                return Ok(ApiResponse<object>.SuccessResponse(_mapper
                 .Map<List<CityWithoutHotelsResponse>>
                 (paginatedListOfCities.Items)));

            return Ok(ApiResponse<object>.SuccessResponse(paginatedListOfCities.Items));
        }

        [HttpGet("{cityId:guid}", Name = "GetCity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCityAsync(Guid cityId)
        {
            var request = new GetCityByIdQuery { Id = cityId};
            var result = await _mediator.Send(request);
            var cityDto = _mapper.Map<CityWithoutHotelsResponse>(result);

            return Ok(ApiResponse<CityWithoutHotelsResponse>.SuccessResponse(cityDto));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCityAsync(CityForCreationRequest city)
        {
            var request = _mapper.Map<CreateCityCommand>(city);
            var createdCity = await _mediator.Send(request);

            var response = ApiResponse<CityWithoutHotelsResponse>.SuccessResponse(createdCity,
                "City created successfully");

            return CreatedAtRoute("GetCity",
            new
            {
                cityId = createdCity.Id
            }, response);
        }

        [HttpPut("{cityId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateCityAsync(Guid cityId, CityForUpdateRequest cityForUpdate)
        {

                var request = _mapper.Map<UpdateCityCommand>(cityForUpdate);
                request.Id = cityId;
                await _mediator.Send(request);

            var response = ApiResponse<object>.SuccessResponse(null, "City updated successfully.");
            return Ok(response);
        }
    }
}