namespace TravelEase.Application.HotelManagement.DTOs.Requests
{
    public record HotelForUpdateRequest
    {
        public Guid CityId { get; set; }
        public string OwnerName { get; set; }
        public string Name { get; set; }
        public float Rating { get; set; }
        public string StreetAddress { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public int FloorsNumber { get; set; }
    }
}