using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Application.Queries.GetNoteById
{
    public class GetNoteByIdQueryValidator : AbstractValidator<GetNoteByIdQuery>
    {
        public GetNoteByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("ID is required");
        }
    }
}
