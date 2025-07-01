using Microsoft.EntityFrameworkCore;
using System;
using TravelEase.Domain.Aggregates.Users;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Infrastructure.Common.Helpers;
using TravelEase.Infrastructure.Persistence.Context;
using TravelEase.Infrastructure.Persistence.CommonRepositories;

namespace TravelEase.Infrastructure.Persistence.EntityPersistence.UserPersistence
{
    public class UserRepository : GenericCrudRepository<User>, IUserRepository
    {
        private readonly TravelEaseDbContext _context;

        public UserRepository(TravelEaseDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PaginatedList<User>> GetAllAsync(bool includeBookings, int pageNumber, int pageSize)
        {
            IQueryable<User> query = _context.Users.AsQueryable();

            if (includeBookings)
            {
                query = query.Include(u => u.Bookings);
            }

            return await PaginationHelper.PaginateAsync(query.AsNoTracking(), pageNumber, pageSize);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}