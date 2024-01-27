using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notepad.Api.Notes.Models.Requests;
using Notepad.Api.Notes.Models.Responses;
using System.Security.Claims;

namespace Notepad.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class NotepadController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotepadController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<NoteResponse>> CreateNote(CreateNoteRequest request)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            request.Username = username ?? string.Empty;

            try
            {
                var response = await _mediator.Send(request);
                return Ok(response);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
        }

        [HttpPatch]
        public async Task<ActionResult<NoteResponse>> UpdateNote(UpdateNoteRequest request)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            request.Username = username ?? string.Empty;

            try
            {
                var response = await _mediator.Send(request);
                return Ok(response);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
        }

        [HttpDelete]
        public async Task<ActionResult<NoteResponse>> DeleteNote(DeleteNoteRequest request)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            request.Username = username ?? string.Empty;

            try
            {
                var response = await _mediator.Send(request);
                return Ok(response);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NoteResponse>>> GetNotes([FromQuery] IEnumerable<string> tags)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            var request = new GetNotesRequest() { Tags = tags };
            request.Username = username ?? string.Empty;

            try
            {
                var response = await _mediator.Send(request);
                return Ok(response);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
        }
    }
}