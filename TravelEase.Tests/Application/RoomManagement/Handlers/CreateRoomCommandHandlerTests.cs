using AutoFixture.AutoMoq;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using TravelEase.Application.RoomManagement.Commands;
using TravelEase.Application.RoomManagement.DTOs.Responses;
using TravelEase.Application.RoomManagement.Handlers;
using TravelEase.Domain.Aggregates.Rooms;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.RoomManagement.Handlers
{
    public class CreateRoomCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IOwnershipValidator> _ownershipValidatorMock = new();
        private readonly CreateRoomCommandHandler _handler;
        private readonly Fixture _fixture = new();

        public CreateRoomCommandHandlerTests()
        {
            _handler = new CreateRoomCommandHandler(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _ownershipValidatorMock.Object
            );

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldCreateRoom_WhenValidRequest()
        {
            var command = _fixture.Create<CreateRoomCommand>();
            var roomEntity = _fixture.Create<Room>();
            var response = _fixture.Create<RoomResponse>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.HotelId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.RoomTypes.ExistsAsync(command.RoomTypeId)).ReturnsAsync(true);
            _ownershipValidatorMock.Setup(v =>
                v.IsRoomTypeBelongsToHotelAsync(command.RoomTypeId, command.HotelId)).ReturnsAsync(true);

            _mapperMock.Setup(m => m.Map<Room>(command)).Returns(roomEntity);
            _unitOfWorkMock.Setup(u => u.Rooms.AddAsync(roomEntity)).ReturnsAsync(roomEntity);
            _mapperMock.Setup(m => m.Map<RoomResponse>(roomEntity)).Returns(response);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(response);

            _unitOfWorkMock.Verify(u => u.Rooms.AddAsync(roomEntity), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenHotelDoesNotExist()
        {
            var command = _fixture.Create<CreateRoomCommand>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.HotelId)).ReturnsAsync(false);

            var act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Hotel doesn't exist.");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenRoomTypeDoesNotExist()
        {
            var command = _fixture.Create<CreateRoomCommand>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.HotelId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.RoomTypes.ExistsAsync(command.RoomTypeId)).ReturnsAsync(false);

            var act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Room category doesn't exist.");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenRoomTypeDoesNotBelongToHotel()
        {
            var command = _fixture.Create<CreateRoomCommand>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.HotelId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.RoomTypes.ExistsAsync(command.RoomTypeId)).ReturnsAsync(true);
            _ownershipValidatorMock.Setup(v =>
                v.IsRoomTypeBelongsToHotelAsync(command.RoomTypeId, command.HotelId)).ReturnsAsync(false);

            var act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"RoomType with ID {command.RoomTypeId} does not belong to hotel {command.HotelId}.");
        }
    }
}