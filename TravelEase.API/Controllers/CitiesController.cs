using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TravelEase.API.Common.Responses;
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
    }
}