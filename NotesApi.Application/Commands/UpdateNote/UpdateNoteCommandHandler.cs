using MediatR;
using Microsoft.Extensions.Logging;
using NotesApi.Domain.Common.Exceptions;
using NotesApi.Application.Interfaces;
using NotesApi.Domain.Models;

namespace NotesApi.Application.Commands.UpdateNote
{
    public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand, Unit>
    {
        private readonly ILogger<UpdateNoteCommandHandler> _logger;
        private readonly INoteRepository _repo;
        
        public UpdateNoteCommandHandler(
            ILogger<UpdateNoteCommandHandler> logger,
            INoteRepository repo)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateNoteCommand command, CancellationToken ct = default)
        {
            var note = await _repo.GetByIdAsync(command.Id, ct)
                ?? throw new NotFoundException($"NOTE entity with ID: [{command.Id}] was not found");

            note.Update(command.Title, command.Content);
            await _repo.UpdateAsync(note, ct);

            _logger.LogInformation("NOTE UPDATED, participant entity ID: {0}", note.Id);
            return Unit.Value;
        }
    }
}
