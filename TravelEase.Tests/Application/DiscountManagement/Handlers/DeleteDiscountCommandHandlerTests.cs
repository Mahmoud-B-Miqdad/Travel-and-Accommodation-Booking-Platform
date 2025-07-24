using AutoFixture;
using FluentAssertions;
using Moq;
using TravelEase.Application.DiscountManagement.Commands;
using TravelEase.Application.DiscountManagement.Handlers;
using TravelEase.Domain.Aggregates.Discounts;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.DiscountManagement.Handlers
{
    public class DeleteDiscountCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IOwnershipValidator> _ownershipValidatorMock = new();
        private readonly DeleteDiscountCommandHandler _handler;
        private readonly Fixture _fixture = new();

        public DeleteDiscountCommandHandlerTests()
        {
            _handler = new DeleteDiscountCommandHandler(_unitOfWorkMock.Object, _ownershipValidatorMock.Object);

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenRoomTypeDoesNotExist()
        {
            var command = _fixture.Create<DeleteDiscountCommand>();

            _unitOfWorkMock.Setup(u => u.RoomTypes.ExistsAsync(command.RoomTypeId))
                .ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("RoomType doesn't exist.");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenDiscountDoesNotExist()
        {
            var command = _fixture.Create<DeleteDiscountCommand>();

            _unitOfWorkMock.Setup(u => u.RoomTypes.ExistsAsync(command.RoomTypeId))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(u => u.Discounts.GetByIdAsync(command.DiscountId))
                .ReturnsAsync((Discount?)null);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Discount doesn't exist to delete.");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenDiscountDoesNotBelongToRoomType()
        {
            var command = _fixture.Create<DeleteDiscountCommand>();

            _unitOfWorkMock.Setup(u => u.RoomTypes.ExistsAsync(command.RoomTypeId))
                .ReturnsAsync(true);

            var discount = _fixture.Create<Discount>();

            _unitOfWorkMock.Setup(u => u.Discounts.GetByIdAsync(command.DiscountId))
                .ReturnsAsync(discount);

            _ownershipValidatorMock.Setup(v => v.IsDiscountBelongsToRoomTypeAsync
            (command.DiscountId, command.RoomTypeId)).ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Discount with ID {command.DiscountId} does not belong to roomType {command.RoomTypeId}.");
        }

        [Fact]
        public async Task Handle_ShouldRemoveDiscountAndSave_WhenValid()
        {
            var command = _fixture.Create<DeleteDiscountCommand>();
            var discount = _fixture.Create<Discount>();

            _unitOfWorkMock.Setup(u => u.RoomTypes.ExistsAsync(command.RoomTypeId))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(u => u.Discounts.GetByIdAsync(command.DiscountId))
                .ReturnsAsync(discount);

            _ownershipValidatorMock.Setup(v => v.IsDiscountBelongsToRoomTypeAsync(command.DiscountId, command.RoomTypeId))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(u => u.Discounts.Remove(discount));
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(CancellationToken.None))
                .ReturnsAsync(1);

            await _handler.Handle(command, CancellationToken.None);

            _unitOfWorkMock.Verify(u => u.Discounts.Remove(discount), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
        }
    }
}