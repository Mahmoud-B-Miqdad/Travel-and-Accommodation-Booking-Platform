using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TravelEase.Infrastructure.Common.Helpers;


namespace TravelEase.Tests.Infrastructure.Common.Helpers
{
    public class PaginationHelperTests
    {
        private class FakeEntity
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        private DbContextOptions<TestDbContext> _dbContextOptions;

        public PaginationHelperTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: "PaginationTestDb")
                .Options;

            using var context = new TestDbContext(_dbContextOptions);
            context.Database.EnsureDeleted();
            context.FakeEntities.AddRange(Enumerable.Range(1, 50)
                .Select(i => new FakeEntity { Id = i, Name = $"Entity {i}" }));
            context.SaveChanges();
        }

        [Fact]
        public async Task PaginateAsync_ShouldReturnCorrectPage()
        {
            using var context = new TestDbContext(_dbContextOptions);
            var query = context.FakeEntities.OrderBy(e => e.Id).AsQueryable();

            var result = await PaginationHelper.PaginateAsync(query, pageNumber: 2, pageSize: 10);

            result.Items.Should().HaveCount(10);
            result.Items.First().Id.Should().Be(11);
            result.PageData.TotalItems.Should().Be(50);
            result.PageData.TotalPages.Should().Be(5);
            result.PageData.CurrentPage.Should().Be(2);
        }

        [Fact]
        public async Task PaginateAsync_ShouldReturnEmpty_WhenPageIsTooHigh()
        {
            using var context = new TestDbContext(_dbContextOptions);
            var query = context.FakeEntities.AsQueryable();

            var result = await PaginationHelper.PaginateAsync(query, pageNumber: 10, pageSize: 10);

            result.Items.Should().BeEmpty();
            result.PageData.TotalItems.Should().Be(50);
            result.PageData.TotalPages.Should().Be(5);
            result.PageData.CurrentPage.Should().Be(10);
        }

        private class TestDbContext : DbContext
        {
            public TestDbContext(DbContextOptions<TestDbContext> options)
                : base(options) { }

            public DbSet<FakeEntity> FakeEntities => Set<FakeEntity>();
        }
    }
}