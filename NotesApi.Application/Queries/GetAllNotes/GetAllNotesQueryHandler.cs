using Mapster;
using MediatR;
using NotesApi.Application.DTO;
using NotesApi.Application.Repository;

namespace NotesApi.Application.Queries.GetAllNotes
{
    public class GetAllNotesQueryHandler : IRequestHandler<GetAllNotesQuery, IEnumerable<NoteDto>>
    {
        private readonly INoteRepository _repo;
        public GetAllNotesQueryHandler(INoteRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<NoteDto>> Handle(GetAllNotesQuery command, CancellationToken ct = default)
        {
            var entities = await _repo.GetAllAsync(ct);
            return entities.Adapt<IEnumerable<NoteDto>>();
        }
    }
}
