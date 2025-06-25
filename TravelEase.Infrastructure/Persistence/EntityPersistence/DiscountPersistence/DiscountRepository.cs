using Microsoft.EntityFrameworkCore;
using System;
using TravelEase.Domain.Aggregates.Discounts;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Infrastructure.Common.Helpers;
using TravelEase.Infrastructure.Persistence.Repositories;

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
    }
}