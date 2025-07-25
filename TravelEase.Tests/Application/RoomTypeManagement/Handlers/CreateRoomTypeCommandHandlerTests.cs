using AutoMapper;
using FluentAssertions;
using Moq;
using TravelEase.Application.RoomTypeManagement.Commands;
using TravelEase.Application.RoomTypeManagement.DTOs.Responses;
using TravelEase.Application.RoomTypeManagement.Handlers;
using TravelEase.Domain.Aggregates.RoomAmenities;
using TravelEase.Domain.Aggregates.RoomTypes;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Enums;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.RoomTypeManagement.Handlers
{
    public class CreateRoomTypeCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly CreateRoomTypeCommandHandler _handler;

        public CreateRoomTypeCommandHandlerTests()
        {
            _handler = new CreateRoomTypeCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenHotelDoesNotExist()
        {
            var command = new CreateRoomTypeCommand
            {
                HotelId = Guid.NewGuid(),
                Category = RoomCategory.Double
            };

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.HotelId)).ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Hotel doesn't exist.");
        }

        [Fact]
        public async Task Handle_ShouldThrowConflictException_WhenRoomTypeDuplicate()
        {
            var command = new CreateRoomTypeCommand
            {
                HotelId = Guid.NewGuid(),
                Category = RoomCategory.Single
            };

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.HotelId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.RoomTypes.ExistsByHotelAndCategoryAsync
            (command.HotelId, command.Category)).ReturnsAsync(true);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ConflictException>()
                .WithMessage($"RoomType of category '{command.Category}' already exists for this hotel.");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenAmenityNotFound()
        {
            var command = new CreateRoomTypeCommand
            {
                HotelId = Guid.NewGuid(),
                Category = RoomCategory.Suite,
                AmenityIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
            };

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.HotelId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.RoomTypes.ExistsByHotelAndCategoryAsync
            (command.HotelId, command.Category)).ReturnsAsync(false);

            _unitOfWorkMock.Setup(u => u.RoomAmenities.GetByIdsAsync(command.AmenityIds))
                .ReturnsAsync(new List<RoomAmenity> { new RoomAmenity() });

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("One or more amenities were not found.");
        }

        [Fact]
        public async Task Handle_ShouldAddRoomTypeSuccessfully()
        {
            var command = new CreateRoomTypeCommand
            {
                HotelId = Guid.NewGuid(),
                Category = RoomCategory.Suite,
                AmenityIds = new List<Guid> { Guid.NewGuid() }
            };

            var amenities = new List<RoomAmenity> { new RoomAmenity { Id = command.AmenityIds[0] } };
            var roomTypeMapped = new RoomType();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(command.HotelId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.RoomTypes.ExistsByHotelAndCategoryAsync(command.HotelId, command.Category))
                .ReturnsAsync(false);

            _unitOfWorkMock.Setup(u => u.RoomAmenities.GetByIdsAsync(command.AmenityIds))
                .ReturnsAsync(amenities);

            _mapperMock.Setup(m => m.Map<RoomType>(command)).Returns(roomTypeMapped);
            _unitOfWorkMock.Setup(u => u.RoomTypes.AddAsync(roomTypeMapped))
                .ReturnsAsync(roomTypeMapped);

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            _mapperMock.Setup(m => m.Map<RoomTypeResponse>(roomTypeMapped))
                .Returns(new RoomTypeResponse());

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}