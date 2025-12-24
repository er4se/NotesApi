using MediatR;
using Microsoft.Extensions.Logging;
using NotesApi.Domain.Common.Exceptions;
using NotesApi.Application.Interfaces;
using NotesApi.Application.Common;

namespace NotesApi.Application.Commands.DeleteNote
{
    public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand, Unit>
    {
        private readonly ILogger<DeleteNoteCommandHandler> _logger;
        private readonly INoteRepository _repo;
        private readonly ICacheService _cache;

        public DeleteNoteCommandHandler(
            ILogger<DeleteNoteCommandHandler> logger,
            INoteRepository repo,
            ICacheService cache
            )
        {
            _repo = repo;
            _logger = logger;
            _cache = cache;
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

            _logger.LogInformation("NOTE DELETED, participant entity ID: {0}", note.Id);
            return Unit.Value;
        }
    }
}
