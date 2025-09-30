namespace TravelEase.Domain.Exceptions
{
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException(string message = "Access is forbidden.") : base(message)
        {
        }
    }
}