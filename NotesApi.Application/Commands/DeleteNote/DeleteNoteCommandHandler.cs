using MediatR;
using Microsoft.Extensions.Logging;
using NotesApi.Domain.Common.Exceptions;
using NotesApi.Application.Repository;

namespace NotesApi.Application.Commands.DeleteNote
{
    public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand, Unit>
    {
        private readonly ILogger<DeleteNoteCommandHandler> _logger;
        private readonly INoteRepository _repo;

        public DeleteNoteCommandHandler(
            ILogger<DeleteNoteCommandHandler> logger,
            INoteRepository repo)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteNoteCommand command, CancellationToken ct = default)
        {
            var note = await _repo.GetByIdAsync(command.Id, ct)
                ?? throw new NotFoundException($"NOTE entity with ID: [{command.Id}] was not found");

            await _repo.DeleteAsync(note.Id, ct);

            _logger.LogInformation("NOTE DELETED, participant entity ID: {0}", note.Id);
            return Unit.Value;
        }
    }
}
