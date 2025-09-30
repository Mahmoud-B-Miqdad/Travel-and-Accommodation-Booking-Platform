using Microsoft.EntityFrameworkCore;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Infrastructure.Common.Helpers
{
    public static class PaginationHelper
    {
        public static async Task<PaginatedList<T>> PaginateAsync<T>(
            IQueryable<T> query,
            int pageNumber,
            int pageSize) where T : class
        {
            var totalItemCount = await query.CountAsync();
            var pageData = new PageData(totalItemCount, pageSize, pageNumber);

            var result = await query
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return new PaginatedList<T>(result, pageData);
        }
    }
}