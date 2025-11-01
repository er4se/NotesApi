using MediatR;
using NotesApi.Application.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Application.Commands
{
    public class DeleteNoteCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand, bool>
    {
        private readonly INoteRepository _repo;
        public DeleteNoteCommandHandler(INoteRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(DeleteNoteCommand command, CancellationToken ct = default)
        {
            if (await _repo.DeleteAsync(command.Id, ct))
                return true;

            return false;
        }
    }
}
