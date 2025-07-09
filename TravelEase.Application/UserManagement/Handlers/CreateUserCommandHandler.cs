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

        public CreateUserCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            await EnsureEmailIsUniqueAsync(request.Email);

            var user = _mapper.Map<User>(request);
            PrepareUser(user, request.Password);

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task EnsureEmailIsUniqueAsync(string email)
        {
            var exists = await _unitOfWork.Users.ExistsAsync(email);
            if (exists)
                throw new ConflictException($"User with email '{email}' already exists.");
        }

        private void PrepareUser(User user, string password)
        {
            var saltBytes = _passwordHasher.GenerateSalt();
            var hashedPassword = _passwordHasher.GenerateHashedPassword(password, saltBytes);

            if (hashedPassword is null)
                throw new InvalidOperationException("Can't hash user password.");

            user.Id = Guid.NewGuid();
            user.Salt = Convert.ToBase64String(saltBytes);
            user.PasswordHash = hashedPassword;
            user.Role = UserRole.Guest;
        }
    }
}