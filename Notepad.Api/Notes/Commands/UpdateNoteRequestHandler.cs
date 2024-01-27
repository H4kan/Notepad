using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notepad.Core;
using Notepad.Core.Extensions;
using Notepad.Api.Notes.Models.Requests;
using Notepad.Api.Notes.Models.Responses;

namespace Notepad.Api.Notes.Commands
{
    public class UpdateNoteRequestHandler : IRequestHandler<UpdateNoteRequest, NoteResponse>
    {
        private readonly IMyDbContext _context;

        public UpdateNoteRequestHandler(IMyDbContext context)
        {
            _context = context;
        }

        public async Task<NoteResponse> Handle(UpdateNoteRequest request, CancellationToken cancellationToken)
        {
            var note = _context.Notes
                .Include(n => n.User)
                .FirstOrDefault(n => n.Id == request.NoteId);

            if (note == null || note.User.Username != request.Username) {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("NoteId", "Cannot update given note.")
                });
            }

            note.Content = request.Content;
            note.ExtractTags();

            await _context.SaveChangesAsync(cancellationToken);

            return new NoteResponse()
            {
                NoteId = note.Id,
                Content = note.Content,
                Tags = note.Tags.MapToList()
            };
        }

    }
}
