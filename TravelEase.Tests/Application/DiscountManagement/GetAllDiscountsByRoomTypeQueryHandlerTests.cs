using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using TravelEase.Application.DiscountManagement.DTOs.Responses;
using TravelEase.Application.DiscountManagement.Queries;
using TravelEase.Domain.Aggregates.Discounts;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Domain.Exceptions;
using TravelEase.Application.DiscountManagement.Handlers;

namespace TravelEase.Tests.Application.DiscountManagement
{
    public class GetAllDiscountsByRoomTypeQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly GetAllDiscountsByRoomTypeQueryHandler _handler;
        private readonly Fixture _fixture = new();

        public GetAllDiscountsByRoomTypeQueryHandlerTests()
        {
            _handler = new GetAllDiscountsByRoomTypeQueryHandler(
                _unitOfWorkMock.Object, _mapperMock.Object);

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenRoomTypeDoesNotExist()
        {
            var query = _fixture.Create<GetAllDiscountsByRoomTypeQuery>();

            _unitOfWorkMock.Setup(u => u.RoomTypes.ExistsAsync(query.RoomTypeId))
                .ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("RoomType doesn't exist.");
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedPaginatedList_WhenRoomTypeExists()
        {
            var query = _fixture.Create<GetAllDiscountsByRoomTypeQuery>();

            var discounts = _fixture.CreateMany<Discount>(5).ToList();
            var pageData = new PageData(1, 5, 5);
            var paginatedDiscounts = new PaginatedList<Discount>(discounts, pageData);

            var expectedResponses = _fixture.CreateMany<DiscountResponse>(5).ToList();

            _unitOfWorkMock.Setup(u => u.RoomTypes.ExistsAsync(query.RoomTypeId))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(u => u.Discounts.GetAllByRoomTypeIdAsync(
                query.RoomTypeId, query.PageNumber, query.PageSize))
                .ReturnsAsync(paginatedDiscounts);

            _mapperMock.Setup(m => m.Map<List<DiscountResponse>>(discounts))
                .Returns(expectedResponses);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Items.Should().BeEquivalentTo(expectedResponses);
            result.PageData.Should().BeEquivalentTo(pageData);
        }
    }
}