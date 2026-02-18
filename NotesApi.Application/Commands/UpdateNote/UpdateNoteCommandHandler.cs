using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using NotesApi.Application.Common;
using NotesApi.Application.Interfaces;
using NotesApi.Contracts.Events.V1;
using NotesApi.Domain.Common.Exceptions;
using NotesApi.Domain.Models;

namespace NotesApi.Application.Commands.UpdateNote
{
    public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand, Unit>
    {
        private readonly ILogger<UpdateNoteCommandHandler> _logger;
        private readonly INoteRepository _repo;
        private readonly ICacheService _cache;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ICorrelationContext _correlationContext;

        public UpdateNoteCommandHandler(
            ILogger<UpdateNoteCommandHandler> logger,
            INoteRepository repo,
            ICacheService cache,
            IPublishEndpoint publishEndpoint,
            ICorrelationContext correlationContext
            )
        {
            _repo = repo;
            _logger = logger;
            _cache = cache;
            _publishEndpoint = publishEndpoint;
            _correlationContext = correlationContext;
        }

        public async Task<Unit> Handle(UpdateNoteCommand command, CancellationToken ct = default)
        {
            var note = await _repo.GetByIdAsync(command.Id, ct)
                ?? throw new NotFoundException($"NOTE entity with ID: [{command.Id}] was not found");

            note.Update(command.Title, command.Content);
            await _repo.UpdateAsync(note, ct);

            var cacheKeyById = CacheKeys.Notes.ById(note.Id);
            await _cache.RemoveAsync(CacheKeys.Notes.All, ct);
            await _cache.RemoveAsync(cacheKeyById, ct);

            _logger.LogInformation("Cache invalidated for {CacheKey} and {CacheKeyById}",
                CacheKeys.Notes.All, cacheKeyById);

            await _publishEndpoint.Publish(new NoteUpdated
            {
                NoteId = note.Id,
                Title = note.Title,
                UpdatedAt = DateTime.UtcNow,

                CorrelationId = _correlationContext.CorrelationId

            }, ct);

            _logger.LogInformation(
                "Published event {EventType} for note {NoteId}",
                nameof(NoteUpdated),
                note.Id);

            _logger.LogInformation("NOTE UPDATED, participant entity ID: {0}", note.Id);
            return Unit.Value;
        }
    }
}
