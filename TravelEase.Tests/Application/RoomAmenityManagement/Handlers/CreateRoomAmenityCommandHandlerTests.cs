using AutoFixture.AutoMoq;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using TravelEase.Application.RoomAmenityManagement.Commands;
using TravelEase.Application.RoomAmenityManagement.DTOs.Responses;
using TravelEase.Application.RoomAmenityManagement.Handlers;
using TravelEase.Domain.Aggregates.RoomAmenities;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.RoomAmenityManagement.Handlers
{
    public class CreateRoomAmenityCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly CreateRoomAmenityCommandHandler _handler;
        private readonly Fixture _fixture = new();

        public CreateRoomAmenityCommandHandlerTests()
        {
            _handler = new CreateRoomAmenityCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldCreateAmenity_WhenAmenityDoesNotExist()
        {
            var command = _fixture.Create<CreateRoomAmenityCommand>();
            var amenity = _fixture.Create<RoomAmenity>();
            var response = _fixture.Create<RoomAmenityResponse>();

            _unitOfWorkMock.Setup(x => x.RoomAmenities.ExistsAsync(command.Name))
                .ReturnsAsync(false);

            _mapperMock.Setup(x => x.Map<RoomAmenity>(command))
                .Returns(amenity);

            _unitOfWorkMock.Setup(x => x.RoomAmenities.AddAsync(amenity))
                .ReturnsAsync(amenity);

            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            _mapperMock.Setup(x => x.Map<RoomAmenityResponse>(amenity))
                .Returns(response);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().BeEquivalentTo(response);
            _unitOfWorkMock.Verify(x => x.RoomAmenities.ExistsAsync(command.Name), Times.Once);
            _unitOfWorkMock.Verify(x => x.RoomAmenities.AddAsync(amenity), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowConflictException_WhenAmenityAlreadyExists()
        {
            var command = _fixture.Create<CreateRoomAmenityCommand>();

            _unitOfWorkMock.Setup(x => x.RoomAmenities.ExistsAsync(command.Name))
                .ReturnsAsync(true);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ConflictException>()
                .WithMessage($"RoomAmenity with name '{command.Name}' already exists.");

            _unitOfWorkMock.Verify(x => x.RoomAmenities.AddAsync(It.IsAny<RoomAmenity>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}