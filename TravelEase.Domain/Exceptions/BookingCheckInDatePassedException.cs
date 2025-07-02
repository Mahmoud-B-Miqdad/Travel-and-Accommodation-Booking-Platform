namespace TravelEase.Domain.Exceptions
{
    public class BookingCheckInDatePassedException : Exception
    {
        public BookingCheckInDatePassedException(string message) : base(message)
        {
        }
    }
}