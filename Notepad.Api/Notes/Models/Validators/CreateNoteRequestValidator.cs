using FluentValidation;
using Notepad.Api.Notes.Models.Requests;

namespace Notepad.Api.Notes.Models.Validators
{
    public class CreateNoteRequestValidator : AbstractValidator<CreateNoteRequest>
    {
        public CreateNoteRequestValidator()
        {
            RuleFor(x => x.Content).NotEmpty().WithMessage("Content is required.");
        }
    }
}
