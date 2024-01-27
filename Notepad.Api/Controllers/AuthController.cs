using Microsoft.AspNetCore.Mvc;
using MediatR;
using FluentValidation;
using Notepad.Api.Users.Models.Requests;

namespace Notepad.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(RegisterRequest request)
        {
            try
            {
                await _mediator.Send(request);
                return Ok("User successfully created.");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginRequest request)
        {
            try
            {
                string token = await _mediator.Send(request);
                return Ok(token);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
        }
    }
}
