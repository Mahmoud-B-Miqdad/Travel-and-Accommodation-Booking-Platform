using FluentAssertions;
using Moq;
using TravelEase.Application.RoomTypeManagement.Commands;
using TravelEase.Application.RoomTypeManagement.Handlers;
using TravelEase.Domain.Aggregates.RoomTypes;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.RoomTypeManagement.Handlers
{
    public class DeleteRoomTypeCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IOwnershipValidator> _ownershipValidatorMock = new();
        private readonly DeleteRoomTypeCommandHandler _handler;

        public DeleteRoomTypeCommandHandlerTests()
        {
            _handler = new DeleteRoomTypeCommandHandler(
                _unitOfWorkMock.Object,
                _ownershipValidatorMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenHotelDoesNotExist()
        {
            var command = new DeleteRoomTypeCommand
            {
                HotelId = Guid.NewGuid(),
                RoomTypeId = Guid.NewGuid()
            };

            _unitOfWorkMock.Setup(x => x.Hotels.ExistsAsync(command.HotelId))
                .ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Hotel doesn't exist.");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenRoomTypeDoesNotExist()
        {
            var command = new DeleteRoomTypeCommand
            {
                HotelId = Guid.NewGuid(),
                RoomTypeId = Guid.NewGuid()
            };

            _unitOfWorkMock.Setup(x => x.Hotels.ExistsAsync(command.HotelId))
                .ReturnsAsync(true);
            _unitOfWorkMock.Setup(x => x.RoomTypes.GetByIdAsync(command.RoomTypeId))
                .ReturnsAsync((RoomType)null!);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("RoomType doesn't exist to delete.");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenRoomTypeDoesNotBelongToHotel()
        {
            var command = new DeleteRoomTypeCommand
            {
                HotelId = Guid.NewGuid(),
                RoomTypeId = Guid.NewGuid()
            };

            _unitOfWorkMock.Setup(x => x.Hotels.ExistsAsync(command.HotelId))
                .ReturnsAsync(true);
            _unitOfWorkMock.Setup(x => x.RoomTypes.GetByIdAsync(command.RoomTypeId))
                .ReturnsAsync(new RoomType());
            _ownershipValidatorMock.Setup(v => v.IsRoomTypeBelongsToHotelAsync
            (command.RoomTypeId, command.HotelId)).ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"RoomType with ID {command.RoomTypeId} does not belong to hotel {command.HotelId}.");
        }

        [Fact]
        public async Task Handle_ShouldThrowConflictException_WhenRoomTypeHasRooms()
        {
            var command = new DeleteRoomTypeCommand
            {
                HotelId = Guid.NewGuid(),
                RoomTypeId = Guid.NewGuid()
            };

            _unitOfWorkMock.Setup(x => x.Hotels.ExistsAsync(command.HotelId))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(x => x.RoomTypes.GetByIdAsync(command.RoomTypeId))
                .ReturnsAsync(new RoomType());

            _ownershipValidatorMock.Setup(v => v.IsRoomTypeBelongsToHotelAsync
            (command.RoomTypeId, command.HotelId)).ReturnsAsync(true);

            _unitOfWorkMock.Setup(x => x.RoomTypes.HasRoomsAsync(command.RoomTypeId))
                .ReturnsAsync(true);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ConflictException>()
                .WithMessage("Cannot delete RoomType because it has rooms associated with it, which may have active bookings.");
        }

        [Fact]
        public async Task Handle_ShouldRemoveRoomType_WhenAllConditionsAreValid()
        {
            var command = new DeleteRoomTypeCommand
            {
                HotelId = Guid.NewGuid(),
                RoomTypeId = Guid.NewGuid()
            };

            var roomType = new RoomType();

            _unitOfWorkMock.Setup(x => x.Hotels.ExistsAsync(command.HotelId))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(x => x.RoomTypes.GetByIdAsync(command.RoomTypeId))
                .ReturnsAsync(roomType);

            _ownershipValidatorMock.Setup(v => v.IsRoomTypeBelongsToHotelAsync(command.RoomTypeId, command.HotelId))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(x => x.RoomTypes.HasRoomsAsync(command.RoomTypeId))
                .ReturnsAsync(false);

            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            await _handler.Handle(command, CancellationToken.None);

            _unitOfWorkMock.Verify(x => x.RoomTypes.Remove(roomType), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}