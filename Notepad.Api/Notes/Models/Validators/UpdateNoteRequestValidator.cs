using FluentValidation;
using Notepad.Api.Notes.Models.Requests;

namespace Notepad.Api.Notes.Models.Validators
{
    public class UpdateNoteRequestValidator : AbstractValidator<UpdateNoteRequest>
    {
        public UpdateNoteRequestValidator()
        {
            RuleFor(x => x.Content).NotEmpty().WithMessage("Content is required.");
        }
    }
}
