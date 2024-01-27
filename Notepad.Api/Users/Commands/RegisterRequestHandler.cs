using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notepad.Core;
using Notepad.Core.Models;
using Notepad.Api.Users.Models.Requests;

namespace Notepad.Api.Users.Commands
{
    public class RegisterRequestHandler : IRequestHandler<RegisterRequest>
    {
        private readonly IConfiguration _configuration;
        private readonly IMyDbContext _context;


        public RegisterRequestHandler(IConfiguration configuration, IMyDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task Handle(RegisterRequest request, CancellationToken cancellationToken)
        {
            var relatedUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.UserName);
            if (relatedUser != null)
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("Username", "Cannot create user with given username.")
                });
            }

            string passwordHash = BCrypt.Net
                .BCrypt.HashPassword(request.Password);

            var user = new User()
            {
                Username = request.UserName,
                Password = passwordHash
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
