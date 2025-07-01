using MediatR;

namespace TravelEase.Application.UserManagement.Commands
{
    public class SignInCommand : IRequest<string>
    {
        public string Email { get; init; }
        public string Password { get; init; }
    }
}