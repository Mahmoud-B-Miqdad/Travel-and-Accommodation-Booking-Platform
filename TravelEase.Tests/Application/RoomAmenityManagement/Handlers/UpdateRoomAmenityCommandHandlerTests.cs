using AutoFixture.AutoMoq;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using TravelEase.Application.RoomAmenityManagement.Commands;
using TravelEase.Application.RoomAmenityManagement.Handlers;
using TravelEase.Domain.Aggregates.RoomAmenities;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.RoomAmenityManagement.Handlers
{
    public class UpdateRoomAmenityCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly UpdateRoomAmenityCommandHandler _handler;
        private readonly Fixture _fixture = new();

        public UpdateRoomAmenityCommandHandlerTests()
        {
            _handler = new UpdateRoomAmenityCommandHandler(
                _unitOfWorkMock.Object,
                _mapperMock.Object);

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldUpdateAmenity_WhenDataIsValid()
        {
            var command = _fixture.Create<UpdateRoomAmenityCommand>();
            var mappedAmenity = _fixture.Create<RoomAmenity>();

            _unitOfWorkMock.Setup(u => u.RoomAmenities.ExistsAsync(command.Id)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.RoomAmenities.ExistsAsync(command.Name)).ReturnsAsync(false);

            _mapperMock.Setup(m => m.Map<RoomAmenity>(command)).Returns(mappedAmenity);

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            await _handler.Handle(command, default);

            _unitOfWorkMock.Verify(u => u.RoomAmenities.ExistsAsync(command.Id), Times.Once);
            _unitOfWorkMock.Verify(u => u.RoomAmenities.ExistsAsync(command.Name), Times.Once);
            _unitOfWorkMock.Verify(u => u.RoomAmenities.Update(mappedAmenity), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenAmenityDoesNotExist()
        {
            var command = _fixture.Create<UpdateRoomAmenityCommand>();
            _unitOfWorkMock.Setup(u => u.RoomAmenities.ExistsAsync(command.Id)).ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(command, default);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"RoomAmenity with ID {command.Id} doesn't exist to update.");

            _unitOfWorkMock.Verify(u => u.RoomAmenities.ExistsAsync(command.Id), Times.Once);
            _unitOfWorkMock.Verify(u => u.RoomAmenities.ExistsAsync(command.Name), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowConflictException_WhenNameIsNotUnique()
        {
            var command = _fixture.Create<UpdateRoomAmenityCommand>();
            _unitOfWorkMock.Setup(u => u.RoomAmenities.ExistsAsync(command.Id)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.RoomAmenities.ExistsAsync(command.Name)).ReturnsAsync(true);

            Func<Task> act = async () => await _handler.Handle(command, default);

            await act.Should().ThrowAsync<ConflictException>()
                .WithMessage($"Another RoomAmenity with name '{command.Name}' already exists.");

            _unitOfWorkMock.Verify(u => u.RoomAmenities.ExistsAsync(command.Id), Times.Once);
            _unitOfWorkMock.Verify(u => u.RoomAmenities.ExistsAsync(command.Name), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}