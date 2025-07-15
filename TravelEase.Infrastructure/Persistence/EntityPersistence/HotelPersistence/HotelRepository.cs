using Microsoft.EntityFrameworkCore;
using TravelEase.Domain.Aggregates.Hotels;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Infrastructure.Common.Helpers;
using TravelEase.Infrastructure.Persistence.Context;
using TravelEase.Infrastructure.Persistence.CommonRepositories;
using TravelEase.Domain.Aggregates.Rooms;
using TravelEase.Domain.Common.Helpers;
using TravelEase.Domain.Aggregates.Cities;
using TravelEase.Domain.Common.Models.HotelSearchModels;

namespace TravelEase.Infrastructure.Persistence.EntityPersistence.HotelPersistence
{
    public class HotelRepository : GenericCrudRepository<Hotel>, IHotelRepository
    {
        private readonly TravelEaseDbContext _context;
        private readonly IRoomRepository _roomRepository;

        public HotelRepository(TravelEaseDbContext context, IRoomRepository roomRepository)
            : base(context)
        {
            _context = context;
            _roomRepository = roomRepository;
        }

        public async Task<PaginatedList<Hotel>> GetAllAsync(string? searchQuery, int pageNumber, int pageSize)
        {
            var query = _context.Hotels.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                query = query.Where(hotel =>
                    hotel.Name.Contains(searchQuery) ||
                    hotel.Description.Contains(searchQuery) ||
                    hotel.StreetAddress.Contains(searchQuery));
            }

            return await PaginationHelper.PaginateAsync(query.AsNoTracking(), pageNumber, pageSize);
        }

        public async Task<PaginatedList<HotelSearchResult>> HotelSearchAsync
            (HotelSearchParameters searchParams)
        {
            var cityFilterQuery = GetFilteredCitiesQuery(searchParams.CityName);
            var hotelFilterQuery = GetFilteredHotelsQuery(searchParams.StarRate);
            var roomFilterQuery = _roomRepository.GetAvailableRoomsWithCapacity(
                searchParams.Adults,
                searchParams.Children,
                searchParams.CheckInDate,
                searchParams.CheckOutDate);

            var baseQuery = BuildHotelSearchQuery(cityFilterQuery, hotelFilterQuery, roomFilterQuery);

            var pagedRawResult = await PaginationHelper.PaginateAsync
                (baseQuery, searchParams.PageNumber, searchParams.PageSize);

            var finalResults = MapToHotelSearchResponses(pagedRawResult.Items);

            return new PaginatedList<HotelSearchResult>(finalResults, pagedRawResult.PageData);
        }

        private IQueryable<City> GetFilteredCitiesQuery(string? cityName)
        {
            var query = _context.Cities.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(cityName))
            {
                var trimmed = cityName.Trim();
                query = query.Where(city => EF.Functions.Like(city.Name, $"%{trimmed}%"));
            }

            return query;
        }

        private IQueryable<Hotel> GetFilteredHotelsQuery(float minRating)
        {
            return _context.Hotels
                .AsNoTracking()
                .Where(h => h.Rating >= minRating);
        }

        private IQueryable<HotelSearchRawResult> BuildHotelSearchQuery(
            IQueryable<City> cities,
            IQueryable<Hotel> hotels,
            IQueryable<Room> rooms)
        {
            return from city in cities
                   join hotel in hotels on city.Id equals hotel.CityId
                   join roomType in _context.RoomTypes.AsNoTracking() on hotel.Id equals roomType.HotelId
                   join room in rooms on roomType.Id equals room.RoomTypeId
                   select new HotelSearchRawResult
                   {
                       CityId = city.Id,
                       CityName = city.Name,
                       HotelId = hotel.Id,
                       RoomId = room.Id,
                       RoomPricePerNight = roomType.PricePerNight,
                       RoomTypeCategory = roomType.Category,
                       HotelName = hotel.Name,
                       HotelStreetAddress = hotel.StreetAddress,
                       HotelFloorsNumber = hotel.FloorsNumber,
                       HotelRating = hotel.Rating,
                       RoomDiscounts = roomType.Discounts
                   };
        }

        private List<HotelSearchResult> MapToHotelSearchResponses
            (List<HotelSearchRawResult> rawItems)
        {
            return rawItems.Select(item => new HotelSearchResult
            {
                CityId = item.CityId,
                CityName = item.CityName,
                HotelId = item.HotelId,
                RoomId = item.RoomId,
                RoomPricePerNight = item.RoomPricePerNight,
                RoomType = item.RoomTypeCategory.ToString(),
                HotelName = item.HotelName,
                StreetAddress = item.HotelStreetAddress,
                FloorsNumber = item.HotelFloorsNumber,
                Rating = item.HotelRating,
                Discount = DiscountHelper.GetDiscountForDate(item.RoomDiscounts, DateTime.Today)
            }).ToList();
        }
    }
}