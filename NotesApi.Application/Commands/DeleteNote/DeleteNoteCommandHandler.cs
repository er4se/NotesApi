using MediatR;
using NotesApi.Application.Repository;

namespace NotesApi.Application.Commands.DeleteNote
{
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
