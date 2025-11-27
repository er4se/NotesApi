using MediatR;
using Microsoft.Extensions.Logging;
using NotesApi.Application.Repository;
using NotesApi.Domain.Models;

namespace NotesApi.Application.Commands.CreateNote
{
    public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, int>
    {
        private readonly ILogger<CreateNoteCommandHandler> _logger;
        private readonly INoteRepository _repo;

        public CreateNoteCommandHandler(
            ILogger<CreateNoteCommandHandler> logger,
            INoteRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        public async Task<int> Handle(CreateNoteCommand command, CancellationToken ct = default)
        {
            var entity = new Note { Title = command.Title, Content = command.Content, CreatedAt = DateTime.UtcNow };
            await _repo.CreateAsync(entity, ct);

            _logger.LogInformation($"Created note {entity.Id}");

            return entity.Id;
        }
    }
}
