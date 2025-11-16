using MediatR;
using NotesApi.Application.Repository;
using NotesApi.Domain.Models;

namespace NotesApi.Application.Commands.CreateNote
{
    public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, int>
    {
        private readonly INoteRepository _repo;
        public CreateNoteCommandHandler(INoteRepository repo)
        {
            _repo = repo;
        }

        public async Task<int> Handle(CreateNoteCommand command, CancellationToken ct = default)
        {
            var entity = new Note { Title = command.Title, Content = command.Content, CreatedAt = DateTime.UtcNow };
            await _repo.CreateAsync(entity, ct);
            return entity.Id;
        }
    }
}
