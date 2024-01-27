using MediatR;
using Notepad.Api.Notes.Models.Responses;
using System.Text.Json.Serialization;

namespace Notepad.Api.Notes.Models.Requests
{
    public class CreateNoteRequest : IRequest<NoteResponse>
    {
        [JsonIgnore]
        public string Username { get; set; } = string.Empty;

        public required string Content { get; set; }

    }
}
