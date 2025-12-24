using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using NotesApi.Application.Common;
using NotesApi.Application.DTO;
using NotesApi.Application.Interfaces;

namespace NotesApi.Application.Queries.GetAllNotes
{
    public class GetAllNotesQueryHandler : IRequestHandler<GetAllNotesQuery, IEnumerable<NoteDto>>
    {
        private readonly ILogger<GetAllNotesQueryHandler> _logger;
        private readonly INoteRepository _repo;
        private readonly ICacheService _cache;

        public GetAllNotesQueryHandler(
            ILogger<GetAllNotesQueryHandler> logger,
            INoteRepository repo,
            ICacheService cache
            )
        {
            _repo = repo;
            _logger = logger;
            _cache = cache;
        }

        public async Task<IEnumerable<NoteDto>> Handle(GetAllNotesQuery command, CancellationToken ct = default)
        {
            var cacheKey = CacheKeys.Notes.All;

            var cached = await _cache.GetAsync<IEnumerable<NoteDto>>(cacheKey, ct);
            if (cached is not null)
            {
                _logger.LogInformation("Cache hit for {CacheKey}", cacheKey);
                return cached;
            }

            _logger.LogInformation("Cache miss for {CacheKey}", cacheKey);

            var notes = await _repo.GetAllAsync(ct);
            var dto = notes.Adapt<IEnumerable<NoteDto>>();

            await _cache.SetAsync(cacheKey, dto, TimeSpan.FromSeconds(60), ct);
            _logger.LogInformation("Cache set for {CacheKey} with TTL {Ttl} seconds", cacheKey, 60);

            _logger.LogInformation("ALL NOTES EXTRACTED");
            return dto;
        }
    }
}
