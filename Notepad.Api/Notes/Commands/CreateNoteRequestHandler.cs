using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Notepad.Core;
using Notepad.Core.Extensions;
using Notepad.Core.Models;
using Notepad.Api.Notes.Models.Requests;
using Notepad.Api.Notes.Models.Responses;

namespace Notepad.Api.Notes.Commands
{
    public class CreateNoteRequestHandler : IRequestHandler<CreateNoteRequest, NoteResponse>
    {
        private readonly IMyDbContext _context;

        public CreateNoteRequestHandler(IMyDbContext context)
        {
            _context = context;
        }

        public async Task<NoteResponse> Handle(CreateNoteRequest request, CancellationToken cancellationToken)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == request.Username);
            if (user == null)
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("Username", "Cannot create note for given user.")
                });
            }
            var note = new Note()
            {
                Content = request.Content,
                User = user
            };
            note.ExtractTags();
            _context.Notes.Add(note);

            await _context.SaveChangesAsync(cancellationToken);

            return new NoteResponse() {
                NoteId = note.Id,
                Content = note.Content,
                Tags = note.Tags.MapToList()
            };
        }

    }
}
