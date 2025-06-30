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

        /// <summary>
        /// Retrieves a paginated list of cities with optional filtering and hotel inclusion.
        /// </summary>
        /// <param name="cityQuery">Query parameters for filtering, searching, and pagination.</param>
        /// <returns>Paginated list of cities with or without hotel details.</returns>
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
            {
                var result = _mapper.Map<List<CityWithoutHotelsResponse>>(paginatedListOfCities.Items);
                return Ok(ApiResponse<List<CityWithoutHotelsResponse>>.SuccessResponse(result));
            }

            return Ok(ApiResponse<List<CityResponse>>.SuccessResponse(paginatedListOfCities.Items));
        }

        /// <summary>
        /// Retrieves a specific city by its unique identifier.
        /// </summary>
        /// <param name="cityId">The unique identifier of the city.</param>
        /// <returns>The details of the requested city.</returns>
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

        /// <summary>
        /// Creates a new city.
        /// </summary>
        /// <param name="city">The details of the city to be created.</param>
        /// <returns>The created city details.</returns>
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

        /// <summary>
        /// Updates an existing city by its unique identifier.
        /// </summary>
        /// <param name="cityId">The ID of the city to update.</param>
        /// <param name="cityForUpdate">The updated city data.</param>
        /// <returns>200 Response if successful.</returns>
        [HttpPut("{cityId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateCity(Guid cityId, CityForUpdateRequest cityForUpdate)
        {
                var request = _mapper.Map<UpdateCityCommand>(cityForUpdate);
                request.Id = cityId;
                await _mediator.Send(request);

            var response = ApiResponse<string>.SuccessResponse(null, "City updated successfully.");
            return Ok(response);
        }

        /// <summary>
        /// Deletes a specific city by its unique identifier.
        /// </summary>
        /// <param name="cityId">The ID of the city to delete.</param>
        /// <returns>200 Response if deletion is successful.</returns>
        [HttpDelete("{cityId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteCity(Guid cityId)
        {
            var deleteCityCommand = new DeleteCityCommand { Id = cityId };
            await _mediator.Send(deleteCityCommand);

            var response = ApiResponse<string>.SuccessResponse(null, "City deleted successfully.");
            return Ok(response);
        }
    }
}