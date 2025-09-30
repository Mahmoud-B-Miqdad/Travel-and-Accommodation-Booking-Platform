using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TravelEase.Domain.Aggregates.Bookings;
using TravelEase.Domain.Aggregates.Cities;
using TravelEase.Domain.Aggregates.Hotels;
using TravelEase.Domain.Aggregates.Rooms;
using TravelEase.Domain.Aggregates.RoomTypes;
using TravelEase.Domain.Enums;
using TravelEase.Infrastructure.Persistence.Context;
using TravelEase.Infrastructure.Persistence.EntityPersistence.CityPersistence;

namespace TravelEase.Tests.Infrastructure.IntegrationTests.Repositories
{
    public class CityRepositoryTests
    {
        private TravelEaseDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<TravelEaseDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new TravelEaseDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnPaginatedAndFilteredResults_WithHotels()
        {
            using var context = CreateDbContext();

            var city1 = CreateValidCity("London", "UK", "GB", "LDN001");
            var city2 = CreateValidCity("Rome", "Italy", "IT", "ROM001");

            var hotel1 = CreateValidHotel(city1.Id, "Hotel A");
            var hotel2 = CreateValidHotel(city2.Id, "Hotel B");

            await context.Cities.AddRangeAsync(city1, city2);
            await context.Hotels.AddRangeAsync(hotel1, hotel2);
            await context.SaveChangesAsync();

            var repo = new CityRepository(context);

            var result = await repo.GetAllAsync
                (includeHotels: true, searchQuery: "Lon", pageNumber: 1, pageSize: 10);

            result.Items.Should().HaveCount(1);
            result.Items[0].Name.Should().Be("London");
            result.Items[0].Hotels.Should().ContainSingle(h => h.Name == "Hotel A");
        }

        [Fact]
        public async Task GetTrendingCitiesAsync_ShouldReturnCitiesOrderedByBookingCount()
        {
            using var context = CreateDbContext();

            var city1 = CreateValidCity("Paris", "France", "FR", "PRS001");
            var city2 = CreateValidCity("Berlin", "Germany", "DE", "BER001");

            var hotel1 = CreateValidHotel(city1.Id, "Hotel Paris");
            var hotel2 = CreateValidHotel(city2.Id, "Hotel Berlin");

            var roomType1 = CreateValidRoomType(hotel1.Id);
            var roomType2 = CreateValidRoomType(hotel2.Id);

            var room1 = CreateValidRoom(roomType1.Id, "Garden");
            var room2 = CreateValidRoom(roomType2.Id, "City");

            var booking1 = CreateValidBooking(room1.Id, 200);
            var booking2 = CreateValidBooking(room1.Id, 300);
            var booking3 = CreateValidBooking(room2.Id, 150);

            await context.AddRangeAsync(city1, city2, hotel1, hotel2,
                roomType1, roomType2, room1, room2, booking1, booking2, booking3);

            await context.SaveChangesAsync();

            var repo = new CityRepository(context);

            var result = await repo.GetTrendingCitiesAsync(2);

            result.Should().HaveCount(2);
            result[0].Id.Should().Be(city1.Id); // Paris has 2 bookings
            result[1].Id.Should().Be(city2.Id); // Berlin has 1 booking
        }

        #region Helper Methods

        private City CreateValidCity(string name, string country, string code, string postOffice)
        {
            return new City
            {
                Id = Guid.NewGuid(),
                Name = name,
                CountryName = country,
                CountryCode = code,
                PostOffice = postOffice
            };
        }

        private Hotel CreateValidHotel(Guid cityId, string name)
        {
            return new Hotel
            {
                Id = Guid.NewGuid(),
                Name = name,
                CityId = cityId,
                OwnerName = "Owner Name",
                Description = "Some description",
                PhoneNumber = "0123456789",
                StreetAddress = "123 Main St"
            };
        }

        private RoomType CreateValidRoomType(Guid hotelId)
        {
            return new RoomType
            {
                Id = Guid.NewGuid(),
                HotelId = hotelId,
                Category = RoomCategory.Double,
                PricePerNight = 120
            };
        }

        private Room CreateValidRoom(Guid roomTypeId, string view)
        {
            return new Room
            {
                Id = Guid.NewGuid(),
                RoomTypeId = roomTypeId,
                View = view
            };
        }

        private Booking CreateValidBooking(Guid roomId, double price)
        {
            return new Booking
            {
                Id = Guid.NewGuid(),
                RoomId = roomId,
                BookingDate = DateTime.Today,
                CheckInDate = DateTime.Today,
                CheckOutDate = DateTime.Today.AddDays(2),
                Price = price
            };
        }

        #endregion
    }
}