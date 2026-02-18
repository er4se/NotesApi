using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using NotesApi.Application.Common;
using NotesApi.Application.Interfaces;
using NotesApi.Contracts.Events.V1;
using NotesApi.Domain.Common.Exceptions;

namespace NotesApi.Application.Commands.DeleteNote
{
    public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand, Unit>
    {
        private readonly ILogger<DeleteNoteCommandHandler> _logger;
        private readonly INoteRepository _repo;
        private readonly ICacheService _cache;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ICorrelationContext _correlationContext;

        public DeleteNoteCommandHandler(
            ILogger<DeleteNoteCommandHandler> logger,
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

        public async Task<Unit> Handle(DeleteNoteCommand command, CancellationToken ct = default)
        {
            var note = await _repo.GetByIdAsync(command.Id, ct)
                ?? throw new NotFoundException($"NOTE entity with ID: [{command.Id}] was not found");

            await _repo.DeleteAsync(note.Id, ct);

            var cacheKeyById = CacheKeys.Notes.ById(note.Id);
            await _cache.RemoveAsync(CacheKeys.Notes.All, ct);
            await _cache.RemoveAsync(cacheKeyById, ct);

            _logger.LogInformation("Cache invalidated for {CacheKey} and {CacheKeyById}",
                CacheKeys.Notes.All, cacheKeyById);

            await _publishEndpoint.Publish(new NoteDeleted
            {
                NoteId = note.Id,
                DeletedAt = DateTime.UtcNow,

                CorrelationId = _correlationContext.CorrelationId

            }, ct);

            _logger.LogInformation(
                "Published event {EventType} for note {NoteId}",
                nameof(NoteDeleted),
                note.Id);

            _logger.LogInformation("NOTE DELETED, participant entity ID: {0}", note.Id);
            return Unit.Value;
        }
    }
}
