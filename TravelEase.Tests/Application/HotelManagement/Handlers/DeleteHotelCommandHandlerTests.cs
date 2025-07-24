using AutoFixture;
using FluentAssertions;
using Moq;
using TravelEase.Application.HotelManagement.Commands;
using TravelEase.Application.HotelManagement.Handlers;
using TravelEase.Domain.Aggregates.Hotels;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.HotelManagement.Handlers
{
    public class DeleteHotelCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly DeleteHotelCommandHandler _handler;
        private readonly Fixture _fixture = new();

        public DeleteHotelCommandHandlerTests()
        {
            _handler = new DeleteHotelCommandHandler(_unitOfWorkMock.Object);

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenHotelNotFound()
        {
            var command = _fixture.Create<DeleteHotelCommand>();

            _unitOfWorkMock.Setup(u => u.Hotels.GetByIdAsync(command.Id))
                .ReturnsAsync((Hotel)null!);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Hotel doesn't exist to delete.");
        }

        [Fact]
        public async Task Handle_ShouldRemoveHotelAndSave_WhenHotelExists()
        {
            var command = _fixture.Create<DeleteHotelCommand>();
            var hotel = _fixture.Create<Hotel>();

            _unitOfWorkMock.Setup(u => u.Hotels.GetByIdAsync(command.Id))
                .ReturnsAsync(hotel);

            await _handler.Handle(command, CancellationToken.None);

            _unitOfWorkMock.Verify(u => u.Hotels.Remove(hotel), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}