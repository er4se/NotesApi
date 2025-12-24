using Mapster;
using MediatR;
using NotesApi.Application.DTO;
using NotesApi.Application.Interfaces;
using NotesApi.Domain.Common.Exceptions;
using Microsoft.Extensions.Logging;
using NotesApi.Application.Common;

namespace NotesApi.Application.Queries.GetNoteById
{
    public class GetNoteByIdQueryHandler : IRequestHandler<GetNoteByIdQuery, NoteDto>
    {
        private readonly ILogger<GetNoteByIdQueryHandler> _logger;
        private readonly INoteRepository _repo;
        private readonly ICacheService _cache;

        public GetNoteByIdQueryHandler(
            ILogger<GetNoteByIdQueryHandler> logger,
            INoteRepository repo,
            ICacheService cache
            )
        {
            _repo = repo;
            _logger = logger;
            _cache = cache;
        }

        public async Task<NoteDto> Handle(GetNoteByIdQuery command, CancellationToken ct = default)
        {
            var cacheKey = CacheKeys.Notes.ById(command.Id);

            var cached = await _cache.GetAsync<NoteDto>(cacheKey, ct);
            if (cached is not null)
            {
                _logger.LogInformation("Cache hit for {CacheKey}", cacheKey);
                return cached;
            }

            _logger.LogInformation("Cache miss for {CacheKey}", cacheKey);

            var note = await _repo.GetByIdAsync(command.Id, ct)
                ?? throw new NotFoundException($"NOTE entity with ID: [{command.Id}] was not found");
            var dto = note.Adapt<NoteDto>();

            await _cache.SetAsync(cacheKey,
                dto, TimeSpan.FromSeconds(60), ct);

            _logger.LogInformation("Cache set for {CacheKey} with TTL {Ttl} seconds", cacheKey, 60);

            _logger.LogInformation("NOTE EXTRACTED, participant entity ID: {0}", note.Id);
            return dto;
        }
    }
}
