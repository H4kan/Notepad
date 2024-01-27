using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notepad.Core;
using Notepad.Core.Models;
using Notepad.Api.Users.Models.Requests;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Notepad.Api.Users.Commands
{
    public class LoginRequestHandler : IRequestHandler<LoginRequest, string>
    {
        private readonly IConfiguration _configuration;
        private readonly IMyDbContext _context;


        public LoginRequestHandler(IConfiguration configuration, IMyDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<string> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var relatedUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.UserName);
            if (relatedUser == null || !BCrypt.Net
                .BCrypt.Verify(request.Password, relatedUser.Password))
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("Username", "Invalid credentials.")
                });
            }

            return CreateToken(relatedUser);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Authorization:Key").Value!));


            var cred = new Microsoft.IdentityModel.Tokens.SigningCredentials(
                key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

    }
}
