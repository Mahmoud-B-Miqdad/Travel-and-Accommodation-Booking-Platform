namespace TravelEase.Application.BookingManagement.DTOs.Requests
{
    public class BookingQueryRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}