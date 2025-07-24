using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using TravelEase.Application.HotelManagement.Commands;
using TravelEase.Application.HotelManagement.DTOs.Responses;
using TravelEase.Application.HotelManagement.Handlers;
using TravelEase.Domain.Aggregates.Hotels;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.HotelManagement.Handlers
{
    public class CreateHotelCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly CreateHotelCommandHandler _handler;
        private readonly Fixture _fixture = new();

        public CreateHotelCommandHandlerTests()
        {
            _handler = new CreateHotelCommandHandler(
                _unitOfWorkMock.Object,
                _mapperMock.Object);

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldThrowConflictException_WhenHotelWithSameNameExists()
        {
            var command = _fixture.Create<CreateHotelCommand>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.Name))
                .ReturnsAsync(true);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ConflictException>()
                .WithMessage($"Hotel with name '{command.Name}' already exists.");
        }

        [Fact]
        public async Task Handle_ShouldAddHotelAndReturnResponse_WhenAllValidationsPass()
        {
            var command = _fixture.Create<CreateHotelCommand>();
            var hotel = _fixture.Create<Hotel>();
            var addedHotel = _fixture.Create<Hotel>();
            var expectedResponse = _fixture.Create<HotelWithoutRoomsResponse>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.Name))
                .ReturnsAsync(false);

            _mapperMock.Setup(m => m.Map<Hotel>(command))
                .Returns(hotel);

            _unitOfWorkMock.Setup(u => u.Hotels.AddAsync(hotel))
                .ReturnsAsync(addedHotel);

            _mapperMock.Setup(m => m.Map<HotelWithoutRoomsResponse>(addedHotel))
                .Returns(expectedResponse);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResponse);

            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}