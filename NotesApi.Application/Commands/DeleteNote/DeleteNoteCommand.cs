using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Application.Commands.DeleteNote
{
    public class DeleteNoteCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
