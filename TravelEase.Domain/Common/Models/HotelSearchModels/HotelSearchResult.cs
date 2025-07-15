namespace TravelEase.Domain.Common.Models.HotelSearchModels
{
    public record HotelSearchResult
    {
        public Guid CityId { get; set; }
        public string CityName { get; set; }
        public Guid HotelId { get; set; }
        public Guid RoomId { get; set; }
        public float RoomPricePerNight { get; set; }
        public string RoomType { get; set; }
        public string HotelName { get; set; }
        public string StreetAddress { get; set; }
        public int FloorsNumber { get; set; }
        public float Rating { get; set; }
        public float Discount { get; set; }
    }
}