using AutoMapper;
using FluentAssertions;
using Moq;
using TravelEase.Application.RoomTypeManagement.DTOs.Responses;
using TravelEase.Application.RoomTypeManagement.Handlers;
using TravelEase.Application.RoomTypeManagement.Queries;
using TravelEase.Domain.Aggregates.RoomTypes;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.RoomTypeManagement.Handlers
{
    public class GetAllRoomTypesByHotelIdQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly GetAllRoomTypesByHotelIdQueryHandler _handler;

        public GetAllRoomTypesByHotelIdQueryHandlerTests()
        {
            _handler = new GetAllRoomTypesByHotelIdQueryHandler(
                _unitOfWorkMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenHotelDoesNotExist()
        {
            var query = new GetAllRoomTypesByHotelIdQuery
            {
                HotelId = Guid.NewGuid(),
                PageNumber = 1,
                PageSize = 10,
                IncludeAmenities = false
            };

            _unitOfWorkMock.Setup(x => x.Hotels.ExistsAsync(query.HotelId))
                .ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Hotel doesn't exist.");
        }

        [Fact]
        public async Task Handle_ShouldReturnPaginatedRoomTypes_WhenHotelExists()
        {
            var query = new GetAllRoomTypesByHotelIdQuery
            {
                HotelId = Guid.NewGuid(),
                PageNumber = 1,
                PageSize = 2,
                IncludeAmenities = true
            };

            var roomTypes = new List<RoomType>
            {
                new RoomType(),
                new RoomType()
            };

            var pageData = new PageData(1, 2, 2);
            var paginatedRoomTypes = new PaginatedList<RoomType>(roomTypes, pageData);

            var mappedResponse = new List<RoomTypeResponse>
            {
                new RoomTypeResponse(),
                new RoomTypeResponse()
            };

            _unitOfWorkMock.Setup(x => x.Hotels.ExistsAsync(query.HotelId))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(x => x.RoomTypes.GetAllByHotelIdAsync(
                    query.HotelId,
                    query.IncludeAmenities,
                    query.PageNumber,
                    query.PageSize))
                .ReturnsAsync(paginatedRoomTypes);

            _mapperMock.Setup(m => m.Map<List<RoomTypeResponse>>(roomTypes))
                .Returns(mappedResponse);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
            result.PageData.Should().BeEquivalentTo(pageData);
        }
    }
}