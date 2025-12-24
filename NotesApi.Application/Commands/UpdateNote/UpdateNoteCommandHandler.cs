using MediatR;
using Microsoft.Extensions.Logging;
using NotesApi.Application.Common;
using NotesApi.Application.Interfaces;
using NotesApi.Domain.Common.Exceptions;
using NotesApi.Domain.Models;

namespace NotesApi.Application.Commands.UpdateNote
{
    public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand, Unit>
    {
        private readonly ILogger<UpdateNoteCommandHandler> _logger;
        private readonly INoteRepository _repo;
        private readonly ICacheService _cache;
        
        public UpdateNoteCommandHandler(
            ILogger<UpdateNoteCommandHandler> logger,
            INoteRepository repo,
            ICacheService cache
            )
        {
            _repo = repo;
            _logger = logger;
            _cache = cache;
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

            _logger.LogInformation("NOTE UPDATED, participant entity ID: {0}", note.Id);
            return Unit.Value;
        }
    }
}
