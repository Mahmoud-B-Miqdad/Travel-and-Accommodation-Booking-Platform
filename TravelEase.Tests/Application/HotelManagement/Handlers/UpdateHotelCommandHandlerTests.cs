using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using TravelEase.Application.HotelManagement.Commands;
using TravelEase.Application.HotelManagement.Handlers;
using TravelEase.Domain.Aggregates.Hotels;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.HotelManagement.Handlers
{
    public class UpdateHotelCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly UpdateHotelCommandHandler _handler;
        private readonly Fixture _fixture = new();

        public UpdateHotelCommandHandlerTests()
        {
            _handler = new UpdateHotelCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldUpdateHotel_WhenHotelExistsAndNameIsUnique()
        {
            var command = _fixture.Create<UpdateHotelCommand>();
            var hotelEntity = _fixture.Create<Hotel>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.Id)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.Name)).ReturnsAsync(false);
            _mapperMock.Setup(m => m.Map<Hotel>(command)).Returns(hotelEntity);

            await _handler.Handle(command, CancellationToken.None);

            _unitOfWorkMock.Verify(u => u.Hotels.Update(hotelEntity), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenHotelDoesNotExist()
        {
            var command = _fixture.Create<UpdateHotelCommand>();
            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.Id)).ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Hotel with ID {command.Id} doesn't exist to update.");
        }

        [Fact]
        public async Task Handle_ShouldThrowConflictException_WhenNameIsNotUnique()
        {
            var command = _fixture.Create<UpdateHotelCommand>();
            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.Id)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.Name)).ReturnsAsync(true);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ConflictException>()
                .WithMessage($"Another hotel with name '{command.Name}' already exists.");
        }
    }
}