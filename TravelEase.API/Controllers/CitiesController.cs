using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TravelEase.API.Common.Responses;
using TravelEase.Application.CityManagement.Commands;
using TravelEase.Application.CityManagement.DTOs.Requests;
using TravelEase.Application.CityManagement.DTOs.Responses;
using TravelEase.Application.CityManagement.Queries;
using TravelEase.Application.ImageManagement.ForCityEntity.Queries;
using TravelEase.Application.ImageManagement.ForHotelEntity.Queries;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.API.Controllers
{
    [Route("api/cities")]
    [ApiController]
    [ApiVersion("1.0")]

    public class CitiesController : ControllerBase
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
        [ProducesResponseType(typeof(ApiResponse<List<CityWithoutHotelsResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<CityResponse>>), StatusCodes.Status200OK)]
        [Authorize(Policy = "MustBeAdmin")]
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

            var response = ApiResponse<List<CityResponse>>.SuccessResponse(paginatedListOfCities.Items);
            return Ok(response);
        }

        /// <summary>
        /// Retrieves a specific city by its unique identifier.
        /// </summary>
        /// <param name="cityId">The unique identifier of the city.</param>
        /// <returns>The details of the requested city.</returns>
        [HttpGet("{cityId:guid}", Name = "GetCity")]
        [ProducesResponseType(typeof(ApiResponse<CityWithoutHotelsResponse>), StatusCodes.Status200OK)]
        [Authorize(Policy = "MustBeAdmin")]
        public async Task<ActionResult<ApiResponse<CityWithoutHotelsResponse>>> GetCityAsync(Guid cityId)
        {
            var request = new GetCityByIdQuery { Id = cityId};
            var result = await _mediator.Send(request);
            var cityResponse = _mapper.Map<CityWithoutHotelsResponse>(result);

            var response = ApiResponse<CityWithoutHotelsResponse>.SuccessResponse(cityResponse);
            return Ok(response);
        }

        /// <summary>
        /// Creates a new city.
        /// </summary>
        /// <param name="cityRequest">The details of the city to be created.</param>
        /// <returns>The created city details.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<CityWithoutHotelsResponse>), StatusCodes.Status201Created)]
        [Authorize(Policy = "MustBeAdmin")]
        public async Task<ActionResult<ApiResponse<CityWithoutHotelsResponse>>>
            CreateCityAsync(CityForCreationRequest cityRequest)
        {
            var request = _mapper.Map<CreateCityCommand>(cityRequest);
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
        /// <param name="cityForUpdateRequest">The updated city data.</param>
        /// <returns>200 Ok Response if successful.</returns>
        [HttpPut("{cityId:guid}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [Authorize(Policy = "MustBeAdmin")]
        public async Task<ActionResult<ApiResponse<string>>> 
            UpdateCity(Guid cityId, CityForUpdateRequest cityForUpdateRequest)
        {
                var request = _mapper.Map<UpdateCityCommand>(cityForUpdateRequest);
                request.Id = cityId;
                await _mediator.Send(request);

            var response = ApiResponse<string>.SuccessResponse(null, "City updated successfully.");
            return Ok(response);
        }

        /// <summary>
        /// Deletes a specific city by its unique identifier.
        /// </summary>
        /// <param name="cityId">The ID of the city to delete.</param>
        /// <returns>200 Ok Response if deletion is successful.</returns>
        [HttpDelete("{cityId:guid}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [Authorize(Policy = "MustBeAdmin")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteCity(Guid cityId)
        {
            var deleteCityCommand = new DeleteCityCommand { Id = cityId };
            await _mediator.Send(deleteCityCommand);

            var response = ApiResponse<string>.SuccessResponse(null, "City deleted successfully.");
            return Ok(response);
        }

        /// <summary>
        /// Retrieves all photos associated with a city based on its unique identifier (GUID).
        /// </summary>
        /// <param name="cityId">The unique identifier of the city.</param>
        /// <returns>
        /// - 200 OK: If the photos are successfully retrieved.
        /// - 404 Not Found: If the city with the.city given ID does not exist.
        /// - 500 Internal Server Error: If an unexpected error occurs.
        /// </returns>
        [HttpGet("{cityId:guid}/photos")]
        [ProducesResponseType(typeof(ApiResponse<PaginatedList<string>>), StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult<ApiResponse<PaginatedList<string>>>> GetAllCityPhotos
            (Guid cityId, int pageNumber = 1, int pageSize = 10)
        {
            var result = await _mediator.Send(new GetAllCityImagesQuery
            {
                CityId = cityId,
                PageNumber = pageNumber,
                PageSize = pageSize
            });

            var response = ApiResponse<PaginatedList<string>>.SuccessResponse(result);
            return Ok(response);
        }
    }
}