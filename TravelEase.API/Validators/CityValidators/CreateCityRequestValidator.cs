﻿using FluentValidation;
using TravelEase.Application.CityManagement.DTOs.Requests;

namespace TravelEase.API.Validators.CityValidators
{
    public class CreateCityRequestValidator : AbstractValidator<CityForCreationRequest>
    {
        public CreateCityRequestValidator()
        {
            RuleFor(city => city.Name)
                .NotEmpty()
                .WithMessage("Name field shouldn't be empty");

            RuleFor(city => city.CountryCode)
                .NotEmpty()
                .Must(city => city.Length == 3)
                .WithMessage("CountryCode must be exactly 3 characters");

            RuleFor(city => city.PostOffice)
                .NotEmpty()
                .WithMessage("PostOffice field shouldn't be empty");

            RuleFor(city => city.CountryName)
                .NotEmpty()
                .WithMessage("CountryName field shouldn't be empty");
        }
    }
}