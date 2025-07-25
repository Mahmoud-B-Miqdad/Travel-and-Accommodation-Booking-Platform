using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using TravelEase.Application.HotelManagement.DTOs.Responses;
using TravelEase.Application.HotelManagement.Handlers;
using TravelEase.Application.HotelManagement.Queries;
using TravelEase.Domain.Aggregates.Hotels;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.HotelManagement.Handlers
{
    public class GetHotelByIdQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly GetHotelByIdQueryHandler _handler;
        private readonly Fixture _fixture = new();

        public GetHotelByIdQueryHandlerTests()
        {
            _handler = new GetHotelByIdQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);

            _fixture.Behaviors
              .OfType<ThrowingRecursionBehavior>()
              .ToList()
              .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedHotel_WhenHotelExists()
        {
            var query = _fixture.Create<GetHotelByIdQuery>();
            var hotel = _fixture.Create<Hotel>();
            var response = _fixture.Create<HotelWithoutRoomsResponse>();

            _unitOfWorkMock.Setup(u => u.Hotels.GetByIdAsync(query.Id))
                .ReturnsAsync(hotel);

            _mapperMock.Setup(m => m.Map<HotelWithoutRoomsResponse>(hotel))
                .Returns(response);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(response);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenHotelDoesNotExist()
        {
            var query = _fixture.Create<GetHotelByIdQuery>();

            _unitOfWorkMock.Setup(u => u.Hotels.GetByIdAsync(query.Id))
                .ReturnsAsync((Hotel?)null);

            var act = () => _handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Hotel with Id {query.Id} was not found.");
        }
    }
}