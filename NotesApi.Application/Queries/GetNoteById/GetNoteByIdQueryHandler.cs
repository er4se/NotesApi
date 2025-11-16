using Mapster;
using MediatR;
using NotesApi.Application.DTO;
using NotesApi.Application.Repository;

namespace NotesApi.Application.Queries.GetNoteById
{
    public class GetNoteByIdQueryHandler : IRequestHandler<GetNoteByIdQuery, NoteDto?>
    {
        private readonly INoteRepository _repo;
        public GetNoteByIdQueryHandler(INoteRepository repo)
        {
            _repo = repo;
        }

        public async Task<NoteDto?> Handle(GetNoteByIdQuery command, CancellationToken ct = default)
        {
            var result = await _repo.GetByIdAsync(command.Id, ct);
            return result.Adapt<NoteDto>();
        }
    }
}
