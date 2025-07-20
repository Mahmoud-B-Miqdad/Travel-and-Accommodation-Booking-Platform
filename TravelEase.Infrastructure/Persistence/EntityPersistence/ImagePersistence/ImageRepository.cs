using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TravelEase.Domain.Aggregates.Images;
using TravelEase.Infrastructure.Persistence.CommonRepositories;

namespace TravelEase.Infrastructure.Persistence.EntityPersistence.ImagePersistence
{
    public class ImageRepository : GenericCrudRepository<Image>, IImageRepository
    {
        public ImageRepository(DbContext context) : base(context)
        {
        }

        public async Task<List<string>> GetAllImageUrlsByEntityIdAsync(Guid entityId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(image => image.EntityId == entityId)
                .Select(image => image.Url)
                .ToListAsync();
        }

        public async Task<Image?> GetSingleOrDefaultAsync(Expression<Func<Image, bool>> predicate)
        {
            return await _dbSet
                .AsNoTracking()
                .SingleOrDefaultAsync(predicate);
        }
    }
}