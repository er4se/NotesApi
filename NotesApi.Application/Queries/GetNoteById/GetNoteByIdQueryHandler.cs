using Mapster;
using MediatR;
using NotesApi.Application.DTO;
using NotesApi.Application.Interfaces;
using NotesApi.Domain.Common.Exceptions;
using Microsoft.Extensions.Logging;

namespace NotesApi.Application.Queries.GetNoteById
{
    public class GetNoteByIdQueryHandler : IRequestHandler<GetNoteByIdQuery, NoteDto>
    {
        private readonly ILogger<GetNoteByIdQueryHandler> _logger;
        private readonly INoteRepository _repo;
        public GetNoteByIdQueryHandler(
            ILogger<GetNoteByIdQueryHandler> logger,
            INoteRepository repo)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<NoteDto> Handle(GetNoteByIdQuery command, CancellationToken ct = default)
        {
            var note = await _repo.GetByIdAsync(command.Id, ct)
                ?? throw new NotFoundException($"NOTE entity with ID: [{command.Id}] was not found");

            _logger.LogInformation("NOTE EXTRACTED, participant entity ID: {0}", note.Id);
            return note.Adapt<NoteDto>();
        }
    }
}
