using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using TravelEase.Application.DiscountManagement.Commands;
using TravelEase.Application.DiscountManagement.DTOs.Responses;
using TravelEase.Application.DiscountManagement.Handlers;
using TravelEase.Domain.Aggregates.Discounts;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.UnitTests.DiscountManagement.Handlers
{
    public class CreateDiscountCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly CreateDiscountCommandHandler _handler;
        private readonly Fixture _fixture = new();

        public CreateDiscountCommandHandlerTests()
        {
            _handler = new CreateDiscountCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenRoomTypeDoesNotExist()
        {
            var command = _fixture.Create<CreateDiscountCommand>();

            _unitOfWorkMock.Setup(u => u.RoomTypes.ExistsAsync(command.RoomTypeId))
                .ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("RoomType doesn't exist.");
        }

        [Fact]
        public async Task Handle_ShouldThrowConflictException_WhenConflictingDiscountExists()
        {
            var command = _fixture.Create<CreateDiscountCommand>();

            _unitOfWorkMock.Setup(u => u.RoomTypes.ExistsAsync(command.RoomTypeId))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(u => u.Discounts.ExistsConflictingDiscountAsync(
                command.RoomTypeId, command.FromDate, command.ToDate))
                .ReturnsAsync(true);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ConflictException>()
                .WithMessage("There is already an overlapping discount for the same room type.");
        }

        [Fact]
        public async Task Handle_ShouldAddDiscountAndReturnResponse_WhenValid()
        {
            var command = _fixture.Create<CreateDiscountCommand>();
            var discount = _fixture.Create<Discount>();
            var discountResponse = _fixture.Create<DiscountResponse>();

            _unitOfWorkMock.Setup(u => u.RoomTypes.ExistsAsync(command.RoomTypeId))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(u => u.Discounts.ExistsConflictingDiscountAsync(
                command.RoomTypeId, command.FromDate, command.ToDate))
                .ReturnsAsync(false);

            _mapperMock.Setup(m => m.Map<Discount>(command))
                .Returns(discount);

            _unitOfWorkMock.Setup(u => u.Discounts.AddAsync(discount))
                .ReturnsAsync(discount);

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(CancellationToken.None))
                .ReturnsAsync(1);

            _mapperMock.Setup(m => m.Map<DiscountResponse>(discount))
                .Returns(discountResponse);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().BeEquivalentTo(discountResponse);

            _unitOfWorkMock.Verify(u => u.Discounts.AddAsync(discount), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
        }
    }
}