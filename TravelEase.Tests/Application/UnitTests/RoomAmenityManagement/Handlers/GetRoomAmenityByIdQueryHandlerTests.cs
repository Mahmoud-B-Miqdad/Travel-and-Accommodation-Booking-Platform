using AutoFixture.AutoMoq;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using TravelEase.Application.RoomAmenityManagement.DTOs.Responses;
using TravelEase.Application.RoomAmenityManagement.Handlers;
using TravelEase.Domain.Aggregates.RoomAmenities;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;
using TravelEase.Application.RoomAmenityManagement.Queries;

namespace TravelEase.Tests.Application.UnitTests.RoomAmenityManagement.Handlers
{
    public class GetRoomAmenityByIdQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly GetRoomAmenityByIdQueryHandler _handler;
        private readonly Fixture _fixture = new();

        public GetRoomAmenityByIdQueryHandlerTests()
        {
            _handler = new GetRoomAmenityByIdQueryHandler(
                _unitOfWorkMock.Object,
                _mapperMock.Object);

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldReturnRoomAmenityResponse_WhenAmenityExists()
        {
            var query = _fixture.Create<GetRoomAmenityByIdQuery>();
            var roomAmenity = _fixture.Create<RoomAmenity>();
            var mappedResponse = _fixture.Create<RoomAmenityResponse>();

            _unitOfWorkMock.Setup(u => u.RoomAmenities.GetByIdAsync(query.Id))
                .ReturnsAsync(roomAmenity);

            _mapperMock.Setup(m => m.Map<RoomAmenityResponse>(roomAmenity))
                .Returns(mappedResponse);

            var result = await _handler.Handle(query, default);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(mappedResponse);

            _unitOfWorkMock.Verify(u => u.RoomAmenities.GetByIdAsync(query.Id), Times.Once);
            _mapperMock.Verify(m => m.Map<RoomAmenityResponse>(roomAmenity), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenAmenityDoesNotExist()
        {
            var query = _fixture.Create<GetRoomAmenityByIdQuery>();

            _unitOfWorkMock.Setup(u => u.RoomAmenities.GetByIdAsync(query.Id))
                .ReturnsAsync((RoomAmenity?)null);

            Func<Task> act = async () => await _handler.Handle(query, default);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Amenity with Id {query.Id} was not found.");

            _unitOfWorkMock.Verify(u => u.RoomAmenities.GetByIdAsync(query.Id), Times.Once);
            _mapperMock.Verify(m => m.Map<RoomAmenityResponse>(It.IsAny<RoomAmenity>()), Times.Never);
        }
    }
}