using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelEase.API.Common.Responses;
using TravelEase.Application.UserManagement.Commands;
using TravelEase.Application.UserManagement.DTOs.Requests;

namespace TravelEase.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UsersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Registers a new user with the provided credentials.
        /// </summary>
        /// <param name="UserForCreationRequest">User registration details.</param>
        /// <returns>An action result indicating success or failure of the registration process.</returns>
        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<string>>> Register(UserForCreationRequest UserForCreationRequest)
        {
            var request = _mapper.Map<CreateUserCommand>(UserForCreationRequest);
            await _mediator.Send(request);

            var response = ApiResponse<string>.SuccessResponse(null, "User registered successfully.");
            return Created(string.Empty, response);
        }
    }
}
