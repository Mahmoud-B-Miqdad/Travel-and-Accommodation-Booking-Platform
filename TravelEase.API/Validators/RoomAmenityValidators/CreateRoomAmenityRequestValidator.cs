﻿using FluentValidation;
using TravelEase.Application.RoomAmenityManagement.DTOs.Requests;

namespace TravelEase.API.Validators.RoomAmenityValidators
{
    public class CreateRoomAmenityRequestValidator : AbstractValidator<RoomAmenityForCreationRequest>
    {
        public CreateRoomAmenityRequestValidator()
        {
            RuleFor(roomAmenity => roomAmenity.Name)
                .NotEmpty()
                .WithMessage("Name field shouldn't be empty");

            RuleFor(roomAmenity => roomAmenity.Description)
                .NotEmpty()
                .WithMessage("Description field shouldn't be empty");
        }
    }
}