using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TravelEase.API.Common.Responses;
using TravelEase.Application.UserManagement.Commands;
using TravelEase.Application.UserManagement.DTOs.Requests;

namespace TravelEase.API.Controllers
{
    [Route("api/Authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AuthenticationController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Registers a new user with the provided credentials.
        /// </summary>
        /// <param name="appUserForCreationDto">User registration details.</param>
        /// <returns>An action result indicating success or failure of the registration process.</returns>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> Register(UserForCreationRequest appUserForCreationDto)
        {
            var request = _mapper.Map<CreateUserCommand>(appUserForCreationDto);
            await _mediator.Send(request);

            return Ok(ApiResponse<string>.SuccessResponse(null, "Register User Successfully."));
        }
    }
}