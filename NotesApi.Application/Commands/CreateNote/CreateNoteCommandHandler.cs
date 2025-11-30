using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using NotesApi.Application.DTO;
using NotesApi.Application.Repository;
using NotesApi.Domain.Models;

namespace NotesApi.Application.Commands.CreateNote
{
    public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, NoteDto>
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

        public async Task<NoteDto> Handle(CreateNoteCommand command, CancellationToken ct)
        {
            var note = Note.Create(command.Title, command.Content);
            await _repo.CreateAsync(note, ct);

            _logger.LogInformation("NOTE CREATED, participant entity ID: {0}", note.Id);

            return note.Adapt<NoteDto>();
        }
    }
}
