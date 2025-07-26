using AutoMapper;
using FluentAssertions;
using Moq;
using TravelEase.Application.RoomTypeManagement.DTOs.Responses;
using TravelEase.Application.RoomTypeManagement.Handlers;
using TravelEase.Application.RoomTypeManagement.Queries;
using TravelEase.Domain.Aggregates.RoomTypes;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.UnitTests.RoomTypeManagement.Handlers
{
    public class GetRoomTypeByIdAndHotelIdQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IOwnershipValidator> _ownershipValidatorMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly GetRoomTypeByIdAndHotelIdQueryHandler _handler;

        public GetRoomTypeByIdAndHotelIdQueryHandlerTests()
        {
            _handler = new GetRoomTypeByIdAndHotelIdQueryHandler(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _ownershipValidatorMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenHotelDoesNotExist()
        {
            var query = new GetRoomTypeByIdAndHotelIdQuery
            {
                HotelId = Guid.NewGuid(),
                RoomTypeId = Guid.NewGuid()
            };

            _unitOfWorkMock.Setup(x => x.Hotels.ExistsAsync(query.HotelId))
                .ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Hotel with ID {query.HotelId} doesn't exist.");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenRoomTypeDoesNotBelongToHotel()
        {
            var query = new GetRoomTypeByIdAndHotelIdQuery
            {
                HotelId = Guid.NewGuid(),
                RoomTypeId = Guid.NewGuid()
            };

            _unitOfWorkMock.Setup(x => x.Hotels.ExistsAsync(query.HotelId))
                .ReturnsAsync(true);

            _ownershipValidatorMock.Setup(x =>
                    x.IsRoomTypeBelongsToHotelAsync(query.RoomTypeId, query.HotelId))
                .ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"RoomType with ID {query.RoomTypeId} does not belong to hotel {query.HotelId}.");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenRoomTypeNotFound()
        {
            var query = new GetRoomTypeByIdAndHotelIdQuery
            {
                HotelId = Guid.NewGuid(),
                RoomTypeId = Guid.NewGuid()
            };

            _unitOfWorkMock.Setup(x => x.Hotels.ExistsAsync(query.HotelId))
                .ReturnsAsync(true);

            _ownershipValidatorMock.Setup(x =>
                    x.IsRoomTypeBelongsToHotelAsync(query.RoomTypeId, query.HotelId))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(x => x.RoomTypes.GetByIdAsync(query.RoomTypeId))
                .ReturnsAsync((RoomType?)null);

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"RoomType with Id {query.RoomTypeId} was not found.");
        }

        [Fact]
        public async Task Handle_ShouldReturnRoomTypeResponse_WhenAllValidationsPass()
        {
            var query = new GetRoomTypeByIdAndHotelIdQuery
            {
                HotelId = Guid.NewGuid(),
                RoomTypeId = Guid.NewGuid()
            };

            var roomType = new RoomType();
            var expectedResponse = new RoomTypeResponse();

            _unitOfWorkMock.Setup(x => x.Hotels.ExistsAsync(query.HotelId))
                .ReturnsAsync(true);

            _ownershipValidatorMock.Setup(x =>
                    x.IsRoomTypeBelongsToHotelAsync(query.RoomTypeId, query.HotelId))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(x => x.RoomTypes.GetByIdAsync(query.RoomTypeId))
                .ReturnsAsync(roomType);

            _mapperMock.Setup(x => x.Map<RoomTypeResponse>(roomType))
                .Returns(expectedResponse);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResponse);
        }
    }
}