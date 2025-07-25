using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using TravelEase.Application.RoomManagement.DTOs.Responses;
using TravelEase.Application.RoomManagement.Handlers;
using TravelEase.Application.RoomManagement.Queries;
using TravelEase.Domain.Aggregates.Rooms;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.RoomManagement.Handlers
{
    public class GetRoomByIdAndHotelIdQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IOwnershipValidator> _ownershipValidatorMock = new();
        private readonly GetRoomByIdAndHotelIdQueryHandler _handler;
        private readonly Fixture _fixture = new();

        public GetRoomByIdAndHotelIdQueryHandlerTests()
        {
            _handler = new GetRoomByIdAndHotelIdQueryHandler(
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
        public async Task Handle_ShouldReturnRoomResponse_WhenRoomExistsAndBelongsToHotel()
        {
            var query = _fixture.Create<GetRoomByIdAndHotelIdQuery>();
            var room = _fixture.Create<Room>();
            var roomResponse = _fixture.Create<RoomResponse>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(query.HotelId))
                .ReturnsAsync(true);

            _ownershipValidatorMock.Setup(v => v.IsRoomBelongsToHotelAsync(query.RoomId, query.HotelId))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(u => u.Rooms.GetByIdAsync(query.RoomId))
                .ReturnsAsync(room);

            _mapperMock.Setup(m => m.Map<RoomResponse>(room))
                .Returns(roomResponse);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(roomResponse);

            _unitOfWorkMock.Verify(u => u.Hotels.ExistsAsync(query.HotelId), Times.Once);
            _ownershipValidatorMock.Verify(v => v.IsRoomBelongsToHotelAsync(query.RoomId, query.HotelId), Times.Once);
            _unitOfWorkMock.Verify(u => u.Rooms.GetByIdAsync(query.RoomId), Times.Once);
            _mapperMock.Verify(m => m.Map<RoomResponse>(room), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenHotelDoesNotExist()
        {
            var query = _fixture.Create<GetRoomByIdAndHotelIdQuery>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(query.HotelId))
                .ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Hotel with ID {query.HotelId} doesn't exist.");

            _ownershipValidatorMock.Verify(v => v.IsRoomBelongsToHotelAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.Rooms.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenRoomDoesNotBelongToHotel()
        {
            var query = _fixture.Create<GetRoomByIdAndHotelIdQuery>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(query.HotelId))
                .ReturnsAsync(true);

            _ownershipValidatorMock.Setup(v => v.IsRoomBelongsToHotelAsync(query.RoomId, query.HotelId))
                .ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Room with ID {query.RoomId} does not belong to hotel {query.HotelId}.");

            _unitOfWorkMock.Verify(u => u.Rooms.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenRoomNotFound()
        {
            var query = _fixture.Create<GetRoomByIdAndHotelIdQuery>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(query.HotelId))
                .ReturnsAsync(true);

            _ownershipValidatorMock.Setup(v => v.IsRoomBelongsToHotelAsync(query.RoomId, query.HotelId))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(u => u.Rooms.GetByIdAsync(query.RoomId))
                .ReturnsAsync((Room?)null);

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Room with Id {query.RoomId} was not found.");
        }
    }
}