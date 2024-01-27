using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notepad.Core;
using Notepad.Core.Extensions;
using Notepad.Core.Models;
using Notepad.Api.Notes.Models.Requests;
using Notepad.Api.Notes.Models.Responses;

namespace Notepad.Api.Notes.Queries
{
    public class GetNotesRequestHandler : IRequestHandler<GetNotesRequest, IEnumerable<NoteResponse>>
    {
        private readonly IMyDbContext _context;

        public GetNotesRequestHandler(IMyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NoteResponse>> Handle(GetNotesRequest request, CancellationToken cancellationToken)
        {
            var notesQuery = _context.Notes
                .Include(n => n.User)
                .Where(n => n.User.Username == request.Username);

            if (request.Tags.Any())
            {
                var requiredTag = Tag.None;
                var tagsEnumsStrings = Enum.GetValues(typeof(Tag)).Cast<Tag>().ToList()
                    .Select(t => new { Tag = t, Name = t.ToString() });
                foreach(var tag in request.Tags)
                {
                    var matchingTag = tagsEnumsStrings.FirstOrDefault(t => t.Name == tag);
                    // that means user provided string that is not recognizable as tag 
                    if (matchingTag == null)
                    {
                        return new List<NoteResponse>();
                    }
                    requiredTag |= matchingTag.Tag;
                }

                // all tags provided by user must match
                notesQuery = notesQuery.Where(n => (n.Tags & requiredTag) == requiredTag);
            }

            var notes = await notesQuery.ToListAsync(cancellationToken);

            return notes.Select(n => new NoteResponse() { 
                Content = n.Content,
                NoteId = n.Id,
                Tags = n.Tags.MapToList(),
            });    
        }

    }
}
