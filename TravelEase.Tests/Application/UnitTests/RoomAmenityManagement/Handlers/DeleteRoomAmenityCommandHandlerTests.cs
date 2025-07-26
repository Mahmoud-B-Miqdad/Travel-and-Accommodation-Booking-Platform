using AutoFixture;
using FluentAssertions;
using Moq;
using TravelEase.Application.RoomAmenityManagement.Commands;
using TravelEase.Application.RoomAmenityManagement.Handlers;
using TravelEase.Domain.Aggregates.RoomAmenities;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.UnitTests.RoomAmenityManagement.Handlers
{
    public class DeleteRoomAmenityCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly DeleteRoomAmenityCommandHandler _handler;
        private readonly Fixture _fixture = new();

        public DeleteRoomAmenityCommandHandlerTests()
        {
            _handler = new DeleteRoomAmenityCommandHandler(_unitOfWorkMock.Object);

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldDeleteAmenity_WhenAmenityExists()
        {
            var command = _fixture.Create<DeleteRoomAmenityCommand>();
            var existingAmenity = _fixture.Create<RoomAmenity>();

            _unitOfWorkMock.Setup(u => u.RoomAmenities.GetByIdAsync(command.Id))
                .ReturnsAsync(existingAmenity);

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            await _handler.Handle(command, CancellationToken.None);

            _unitOfWorkMock.Verify(u => u.RoomAmenities.GetByIdAsync(command.Id), Times.Once);
            _unitOfWorkMock.Verify(u => u.RoomAmenities.Remove(existingAmenity), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenAmenityDoesNotExist()
        {
            var command = _fixture.Create<DeleteRoomAmenityCommand>();

            _unitOfWorkMock.Setup(u => u.RoomAmenities.GetByIdAsync(command.Id))
                .ReturnsAsync((RoomAmenity?)null);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Room Amenity doesn't exist to delete.");

            _unitOfWorkMock.Verify(u => u.RoomAmenities.Remove(It.IsAny<RoomAmenity>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}