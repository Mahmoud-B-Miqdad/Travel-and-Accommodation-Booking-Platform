using AutoMapper;
using MediatR;
using TravelEase.Application.UserManagement.Commands;
using TravelEase.Domain.Aggregates.Users;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Enums;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.UserManagement.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        public CreateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existingEmail = await _unitOfWork.Hotels.IsExistsAsync(request.Email);
            if (existingEmail)
                throw new ConflictException($"User with Email '{request.Email}' already exists.");

            var user = _mapper.Map<User>(request);
            var uniqueUserSalt = _passwordHasher.GenerateSalt();
            var userPasswordHash = _passwordHasher.GenerateHashedPassword(request.Password, uniqueUserSalt);
            if (userPasswordHash is null)
                throw new InvalidOperationException("Can't Hash User Password");

            user.Id = Guid.NewGuid();
            user.PasswordHash = userPasswordHash;
            user.Salt = Convert.ToBase64String(uniqueUserSalt);
            user.Role = UserRole.Guest;
            await _unitOfWork.Users.AddAsync(user);
        }
    }
}