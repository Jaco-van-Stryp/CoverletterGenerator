using CoverletterGenerator.Data;
using Microsoft.EntityFrameworkCore;
using CoverletterGenerator.Entities;
using CoverletterGenerator.Infrastructure.Auth;
using MediatR;

namespace CoverletterGenerator.Features.CreateUser
{
    public class CreateUserHandler(AppDbContext dbContext, IJwtService jwtService)
        : IRequestHandler<CreateUserCommand, CreateUserResult>
    {
        public async Task<CreateUserResult> Handle(
            CreateUserCommand request,
            CancellationToken cancellationToken
        )
        {
            var emailTaken = await dbContext.Users.AnyAsync(
                u => u.Email == request.Email,
                cancellationToken
            );
            if (emailTaken)
                throw new EmailAlreadyExistsException();

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            };

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync(cancellationToken);

            var token = jwtService.GenerateToken(user);
            return new CreateUserResult(user.Id, token);
        }
    }
}
