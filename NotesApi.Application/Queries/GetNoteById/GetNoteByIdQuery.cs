using MediatR;
using NotesApi.Application.DTO;
using NotesApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Application.Queries.GetNoteById
{
    public class GetNoteByIdQuery : IRequest<NoteDto>
    {
        public Guid Id { get; set; }
    }
}
