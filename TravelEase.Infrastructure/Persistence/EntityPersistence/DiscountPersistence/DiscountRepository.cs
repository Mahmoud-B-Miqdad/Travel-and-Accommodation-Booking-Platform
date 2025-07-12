using Microsoft.EntityFrameworkCore;
using System;
using TravelEase.Domain.Aggregates.Discounts;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Infrastructure.Common.Helpers;
using TravelEase.Infrastructure.Persistence.Context;
using TravelEase.Infrastructure.Persistence.CommonRepositories;

namespace TravelEase.Infrastructure.Persistence.EntityPersistence.DiscountPersistence
{
    public class DiscountRepository : GenericCrudRepository<Discount>, IDiscountRepository
    {
        private readonly TravelEaseDbContext _context;

        public DiscountRepository(TravelEaseDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Discount>> GetAllByRoomTypeIdAsync(Guid roomTypeId, int pageNumber, int pageSize)
        {
            var query = _context.Discounts
                .Where(discount => discount.RoomTypeId == roomTypeId);

            return await PaginationHelper.PaginateAsync(query.AsNoTracking(), pageNumber, pageSize);
        }

        public async Task<bool> ExistsConflictingDiscountAsync
            (Guid roomTypeId, DateTime fromDate, DateTime toDate)
        {
            return await _context.Discounts.AnyAsync(discount =>
                discount.RoomTypeId == roomTypeId &&
                discount.FromDate < toDate &&
                discount.ToDate > fromDate
            );
        }
    }
}