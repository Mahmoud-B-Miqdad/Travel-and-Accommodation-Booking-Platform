using MediatR;

namespace TravelEase.Application.UserManagement.Commands
{
    public record CreateUserCommand : IRequest
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
        public string PhoneNumber { get; init; }
    }
}