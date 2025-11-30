using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Application.Commands.CreateNote
{
    public class CreateNoteCommandValidator : AbstractValidator<CreateNoteCommand>
    {
        public CreateNoteCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("TITLE is required")
                .MaximumLength(100).WithMessage("TITLE cannot exceed 100 characters");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("CONTENT is required")
                .MaximumLength(10000).WithMessage("CONTENT cannot exceed 10 000 characters");
        }
    }
}
