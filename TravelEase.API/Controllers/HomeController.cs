using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelEase.API.Common.Responses;
using TravelEase.Application.CityManagement.DTOs.Responses;
using TravelEase.Application.CityManagement.Queries;

namespace TravelEase.API.Controllers
{
    [Route("api/home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves the top 5 trending cities.
        /// </summary>
        /// <returns>The top 5 trending cities.</returns>
        [HttpGet("trending-cities")]
        [ProducesResponseType(typeof(ApiResponse<List<CityWithoutHotelsResponse>>), StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult<ApiResponse<List<CityWithoutHotelsResponse>>>>
            GetTrendingCitiesAsync()
        {
            var request = new GetTrendingCitiesQuery();
            var result = await _mediator.Send(request);
            return Ok(ApiResponse<List<CityWithoutHotelsResponse>>.SuccessResponse(result));
        }
    }
}