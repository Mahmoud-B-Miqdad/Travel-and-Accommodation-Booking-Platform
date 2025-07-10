using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelEase.API.Common.Responses;
using TravelEase.Application.UserManagement.Commands;
using TravelEase.Application.UserManagement.DTOs.Requests;

namespace TravelEase.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AuthController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Endpoint for user sign-in. Validates user credentials and
        /// generates a JWT token upon successful authentication.
        /// </summary>
        /// <param name="request"> Represents the sign-in request containing the user's credentials..</param>
        /// <returns>
        /// If successful, returns the generated JWT token;
        /// otherwise, returns a list of validation errors or unauthorized status.
        /// </returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Login(SignInRequest request)
        {
            var command = _mapper.Map<SignInCommand>(request);
            var token = await _mediator.Send(command);

            var response = ApiResponse<string>.SuccessResponse(token, "Signed in successfully.");
            return Ok(response);
        }
    }
}