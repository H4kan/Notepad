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
    public class DeleteNoteRequestHandler : IRequestHandler<DeleteNoteRequest, NoteResponse>
    {
        private readonly IMyDbContext _context;

        public DeleteNoteRequestHandler(IMyDbContext context)
        {
            _context = context;
        }

        public async Task<NoteResponse> Handle(DeleteNoteRequest request, CancellationToken cancellationToken)
        {
            var note = _context.Notes
                .Include(n => n.User)
                .FirstOrDefault(n => n.Id == request.NoteId);

            if (note == null || note.User.Username != request.Username)
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("NoteId", "Cannot delete given note.")
                });
            }

            _context.Notes.Remove(note);

            await _context.SaveChangesAsync(cancellationToken);

            return new NoteResponse() {
                NoteId = note.Id,
                Content = note.Content,
                Tags = note.Tags.MapToList()
            };
        }

    }
}
