using MediatR;
using NotesApi.Application.DTO;
using NotesApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Application.Queries.GetAllNotes
{
    public class GetAllNotesQuery : IRequest<IEnumerable<NoteDto>> { }
}
