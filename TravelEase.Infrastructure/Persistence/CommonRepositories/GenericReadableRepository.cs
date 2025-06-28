using Microsoft.EntityFrameworkCore;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Infrastructure.Common.Helpers;

namespace TravelEase.Infrastructure.Persistence.CommonRepositories
{
    public class GenericReadableRepository<T> : IReadableRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericReadableRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<PaginatedList<T>> GetAllAsync(int pageNumber, int pageSize)
        {
            var query = _context.Set<T>().AsNoTracking();

            return await PaginationHelper.PaginateAsync(query, pageNumber, pageSize);
        }
    }
}