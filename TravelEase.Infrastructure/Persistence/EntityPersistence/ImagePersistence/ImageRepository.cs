﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TravelEase.Domain.Aggregates.Images;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Infrastructure.Common.Helpers;
using TravelEase.Infrastructure.Persistence.CommonRepositories;
using TravelEase.Infrastructure.Persistence.Context;

namespace TravelEase.Infrastructure.Persistence.EntityPersistence.ImagePersistence
{
    public class ImageRepository : GenericCrudRepository<Image>, IImageRepository
    {
        private readonly TravelEaseDbContext _context;

        public ImageRepository(TravelEaseDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PaginatedList<string>> GetAllImageUrlsByEntityIdAsync
            (Guid entityId, int pageNumber, int pageSize)
        {
            var query = _dbSet
                .Where(image => image.EntityId == entityId)
                .Select(image => image.Url)
                .AsQueryable();

            return await PaginationHelper.PaginateAsync(query, pageNumber, pageSize);
        }

        public async Task<Image?> GetSingleOrDefaultAsync(Expression<Func<Image, bool>> predicate)
        {
            return await _dbSet
                .AsNoTracking()
                .SingleOrDefaultAsync(predicate);
        }
    }
}