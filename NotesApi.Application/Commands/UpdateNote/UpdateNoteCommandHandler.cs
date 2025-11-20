using MediatR;
using NotesApi.Application.Common.Exceptions;
using NotesApi.Application.Repository;
using NotesApi.Domain.Models;

namespace NotesApi.Application.Commands.UpdateNote
{
    public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand, bool>
    {
        private readonly INoteRepository _repo;
        public UpdateNoteCommandHandler(INoteRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(UpdateNoteCommand command, CancellationToken ct = default)
        {
            var entity = new Note { Id = command.Id, Title = command.Title, Content = command.Content };
            if (await _repo.UpdateAsync(entity.Id, entity, ct))
                return true;

            throw new NotFoundException($"Note with ID:{command.Id} was not found");
        }
    }
}
