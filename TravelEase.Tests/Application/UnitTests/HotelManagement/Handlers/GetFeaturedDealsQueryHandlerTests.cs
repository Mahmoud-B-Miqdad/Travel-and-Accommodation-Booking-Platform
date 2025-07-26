using AutoFixture;
using FluentAssertions;
using Moq;
using TravelEase.Application.HotelManagement.Handlers;
using TravelEase.Application.HotelManagement.Queries;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.CommonModels;

namespace TravelEase.Tests.Application.UnitTests.HotelManagement.Handlers
{
    public class GetFeaturedDealsQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly GetFeaturedDealsQueryHandler _handler;
        private readonly Fixture _fixture = new();

        public GetFeaturedDealsQueryHandlerTests()
        {
            _handler = new GetFeaturedDealsQueryHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnListOfFeaturedDeals()
        {
            var query = _fixture.Create<GetFeaturedDealsQuery>();
            var featuredDeals = _fixture.CreateMany<FeaturedDeal>(query.Count).ToList();

            _unitOfWorkMock.Setup(u => u.Hotels.GetFeaturedDealsAsync(query.Count))
                .ReturnsAsync(featuredDeals);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().HaveCount(query.Count);
            result.Should().BeEquivalentTo(featuredDeals);
        }
    }
}