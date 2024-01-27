using MediatR;
using Notepad.Api.Notes.Models.Responses;
using System.Text.Json.Serialization;

namespace Notepad.Api.Notes.Models.Requests
{
    public class GetNotesRequest : IRequest<IEnumerable<NoteResponse>>
    {
        [JsonIgnore]
        public string Username { get; set; } = string.Empty;

        public IEnumerable<string> Tags { get; set; } = new List<string>();

    }
}
