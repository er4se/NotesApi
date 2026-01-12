using Mapster;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using NotesApi.Application.Common;
using NotesApi.Application.DTO;
using NotesApi.Application.Interfaces;
using NotesApi.Contracts.Events.V1;
using NotesApi.Domain.Models;

namespace NotesApi.Application.Commands.CreateNote
{
    public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, NoteDto>
    {
        private readonly ILogger<CreateNoteCommandHandler> _logger;
        private readonly INoteRepository _repo;
        private readonly ICacheService _cache;
        private readonly IPublishEndpoint _publishEndpoint;

        public CreateNoteCommandHandler(
            ILogger<CreateNoteCommandHandler> logger,
            INoteRepository repo,
            ICacheService cache,
            IPublishEndpoint publishEndpoint
            )
        {
            _logger = logger;
            _repo = repo;
            _cache = cache;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<NoteDto> Handle(CreateNoteCommand command, CancellationToken ct)
        {
            var note = Note.Create(command.Title, command.Content, Guid.NewGuid());
            await _repo.CreateAsync(note, ct);

            await _cache.RemoveAsync(CacheKeys.Notes.All, ct);
            _logger.LogInformation("Cache invalidated for {CacheKey}", CacheKeys.Notes.All);

            await _publishEndpoint.Publish(new NoteCreated
            {
                NoteId = note.Id,
                Title = note.Title,
                CreatedAt = note.CreatedAt
            });

            _logger.LogInformation(
                "Published event {EventType} for note {NoteId}",
                nameof(NoteCreated),
                note.Id);

            _logger.LogInformation("NOTE CREATED, participant entity ID: {0}", note.Id);

            return note.Adapt<NoteDto>();
        }
    }
}
