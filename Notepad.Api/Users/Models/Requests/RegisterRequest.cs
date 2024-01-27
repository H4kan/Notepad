using MediatR;

namespace Notepad.Api.Users.Models.Requests
{
    public class RegisterRequest : IRequest
    {
        public required string UserName { get; set; }

        public required string Password { get; set; }
    }
}
