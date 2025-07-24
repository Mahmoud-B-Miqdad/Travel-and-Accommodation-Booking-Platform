using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using TravelEase.Application.DiscountManagement.DTOs.Responses;
using TravelEase.Application.DiscountManagement.Handlers;
using TravelEase.Application.DiscountManagement.Queries;
using TravelEase.Domain.Aggregates.Discounts;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.DiscountManagement.Handlers
{
    public class GetDiscountByIdAndRoomTypeIdQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IOwnershipValidator> _ownershipValidatorMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly GetDiscountByIdAndRoomTypeIdQueryHandler _handler;
        private readonly Fixture _fixture = new();

        public GetDiscountByIdAndRoomTypeIdQueryHandlerTests()
        {
            _handler = new GetDiscountByIdAndRoomTypeIdQueryHandler(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _ownershipValidatorMock.Object);

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenRoomTypeDoesNotExist()
        {
            var query = _fixture.Create<GetDiscountByIdAndRoomTypeIdQuery>();

            _unitOfWorkMock.Setup(u => u.RoomTypes.ExistsAsync(query.RoomTypeId))
                .ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Room Type with ID {query.RoomTypeId} doesn't exist.");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenDiscountDoesNotBelongToRoomType()
        {
            var query = _fixture.Create<GetDiscountByIdAndRoomTypeIdQuery>();

            _unitOfWorkMock.Setup(u => u.RoomTypes.ExistsAsync(query.RoomTypeId))
                .ReturnsAsync(true);

            _ownershipValidatorMock.Setup(o =>
                o.IsDiscountBelongsToRoomTypeAsync(query.DiscountId, query.RoomTypeId))
                .ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Discount with ID {query.DiscountId} does not belong to roomType {query.RoomTypeId}.");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenDiscountNotFound()
        {
            var query = _fixture.Create<GetDiscountByIdAndRoomTypeIdQuery>();

            _unitOfWorkMock.Setup(u => u.RoomTypes.ExistsAsync(query.RoomTypeId))
                .ReturnsAsync(true);

            _ownershipValidatorMock.Setup(o =>
                o.IsDiscountBelongsToRoomTypeAsync(query.DiscountId, query.RoomTypeId))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(u => u.Discounts.GetByIdAsync(query.DiscountId))
                .ReturnsAsync((Discount?)null);

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Discount with ID {query.DiscountId} was not found.");
        }

        [Fact]
        public async Task Handle_ShouldReturnDiscountResponse_WhenAllValidationsPass()
        {
            var query = _fixture.Create<GetDiscountByIdAndRoomTypeIdQuery>();
            var discount = _fixture.Create<Discount>();
            var expectedResponse = _fixture.Create<DiscountResponse>();

            _unitOfWorkMock.Setup(u => u.RoomTypes.ExistsAsync(query.RoomTypeId))
                .ReturnsAsync(true);

            _ownershipValidatorMock.Setup(o =>
                o.IsDiscountBelongsToRoomTypeAsync(query.DiscountId, query.RoomTypeId))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(u => u.Discounts.GetByIdAsync(query.DiscountId))
                .ReturnsAsync(discount);

            _mapperMock.Setup(m => m.Map<DiscountResponse>(discount))
                .Returns(expectedResponse);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResponse);
        }
    }
}