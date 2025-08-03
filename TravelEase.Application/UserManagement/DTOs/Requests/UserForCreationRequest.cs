namespace TravelEase.Application.UserManagement.DTOs.Requests
{
    public record UserForCreationRequest
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
        public string PhoneNumber { get; init; }
    }
}