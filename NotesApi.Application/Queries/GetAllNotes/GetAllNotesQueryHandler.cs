using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using NotesApi.Application.DTO;
using NotesApi.Application.Interfaces;

namespace NotesApi.Application.Queries.GetAllNotes
{
    public class GetAllNotesQueryHandler : IRequestHandler<GetAllNotesQuery, IEnumerable<NoteDto>>
    {
        private readonly ILogger<GetAllNotesQueryHandler> _logger;
        private readonly INoteRepository _repo;

        public GetAllNotesQueryHandler(
            ILogger<GetAllNotesQueryHandler> logger,
            INoteRepository repo)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<IEnumerable<NoteDto>> Handle(GetAllNotesQuery command, CancellationToken ct = default)
        {
            var notes = await _repo.GetAllAsync(ct);

            _logger.LogInformation("ALL NOTES EXTRACTED");
            return notes.Adapt<IEnumerable<NoteDto>>();
        }
    }
}
