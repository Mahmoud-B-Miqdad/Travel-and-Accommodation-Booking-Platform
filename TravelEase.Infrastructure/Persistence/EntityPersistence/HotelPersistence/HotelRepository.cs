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
using TravelEase.Domain.Common.Models.CommonModels;
using TravelEase.Domain.Aggregates.Users;

namespace TravelEase.Infrastructure.Persistence.EntityPersistence.HotelPersistence
{
    public class HotelRepository : GenericCrudRepository<Hotel>, IHotelRepository
    {
        private readonly TravelEaseDbContext _context;
        private readonly IRoomRepository _roomRepository;
        private readonly IUserRepository _userRepository;

        public HotelRepository(TravelEaseDbContext context,
            IRoomRepository roomRepository, IUserRepository userRepository)
            : base(context)
        {
            _context = context;
            _roomRepository = roomRepository;
            _userRepository = userRepository;
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

        public async Task<List<FeaturedDeal>> GetFeaturedDealsAsync(int count)
        {
            var rawDeals = await (
                from city in _context.Cities
                join hotel in _context.Hotels on city.Id equals hotel.CityId
                join roomType in _context.RoomTypes on hotel.Id equals roomType.HotelId
                select new
                {
                    CityName = city.Name,
                    HotelId = hotel.Id,
                    HotelName = hotel.Name,
                    HotelRating = hotel.Rating,
                    BaseRoomPrice = roomType.PricePerNight,
                    RoomClassId = roomType.Id,
                    Discount = roomType.Discounts
                        .FirstOrDefault
                        (d => d.FromDate.Date <= DateTime.Today && d.ToDate.Date >= DateTime.Today)
                }
            ).ToListAsync();

            var featuredDeals = rawDeals.Select(deal =>
            {
                var discountPercentage = deal.Discount?.DiscountPercentage ?? 0f;
                var normalizedDiscount = discountPercentage > 1 ? discountPercentage / 100f : discountPercentage;

                return new FeaturedDeal
                {
                    CityName = deal.CityName,
                    HotelId = deal.HotelId,
                    HotelName = deal.HotelName,
                    HotelRating = deal.HotelRating,
                    BaseRoomPrice = deal.BaseRoomPrice,
                    RoomClassId = deal.RoomClassId,
                    Discount = normalizedDiscount,
                    FinalRoomPrice = deal.BaseRoomPrice * (1 - normalizedDiscount)
                };
            })
            .OrderByDescending(deal => deal.Discount)
            .ThenBy(deal => deal.FinalRoomPrice)
            .Take(count)
            .ToList();

            return featuredDeals;
        }

        public async Task<List<Hotel>> GetRecentlyVisitedHotelsForGuestAsync(Guid guestId, int count)
        {
            return await (from booking in _context.Bookings
                          join room in _context.Rooms on booking.RoomId equals room.Id
                          join roomType in _context.RoomTypes on room.RoomTypeId equals roomType.Id
                          join hotel in _context.Hotels on roomType.HotelId equals hotel.Id
                          where booking.UserId == guestId
                          orderby booking.CheckInDate descending
                          select hotel).Distinct().Take(count)
                .ToListAsync();
        }

        public async Task<List<Hotel>> GetRecentlyVisitedHotelsForAuthenticatedGuestAsync
            (string email, int count)
        {
            var guest = await _userRepository.GetByEmailAsync(email);
            return await GetRecentlyVisitedHotelsForGuestAsync(guest.Id, count);
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