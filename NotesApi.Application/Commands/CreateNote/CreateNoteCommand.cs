using MediatR;
using NotesApi.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Application.Commands.CreateNote
{
    public class CreateNoteCommand : IRequest<NoteDto>
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
