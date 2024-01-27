using MediatR;
using Notepad.Api.Notes.Models.Responses;
using System.Text.Json.Serialization;

namespace Notepad.Api.Notes.Models.Requests
{
    public class DeleteNoteRequest : IRequest<NoteResponse>
    {
        [JsonIgnore]
        public string Username { get; set; } = string.Empty;

        public int NoteId { get; set; }


    }
}
