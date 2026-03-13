using CoverletterGenerator.Data;
using Microsoft.EntityFrameworkCore;
using CoverletterGenerator.Infrastructure.Auth;
using MediatR;

namespace CoverletterGenerator.Features.LoginUser
{
    public class LoginUserHandler(AppDbContext dbContext, IJwtService jwtService)
        : IRequestHandler<LoginUserCommand, LoginUserResult>
    {
        public async Task<LoginUserResult> Handle(
            LoginUserCommand request,
            CancellationToken cancellationToken
        )
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(
                u => u.Email == request.Email,
                cancellationToken
            );

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                throw new InvalidCredentialsException();

            var token = jwtService.GenerateToken(user);
            return new LoginUserResult(user.Id, token);
        }
    }
}
