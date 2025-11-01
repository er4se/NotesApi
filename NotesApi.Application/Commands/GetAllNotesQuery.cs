using Mapster;
using MediatR;
using NotesApi.Application.DTO;
using NotesApi.Application.Repository;
using NotesApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Application.Commands
{
    public class GetAllNotesQuery : IRequest<IEnumerable<NoteDto>> { }

    public class GetAllNotesQueryHandler : IRequestHandler<GetAllNotesQuery, IEnumerable<NoteDto>>
    {
        private readonly INoteRepository _repo;
        public GetAllNotesQueryHandler(INoteRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<NoteDto>>Handle(GetAllNotesQuery command, CancellationToken ct = default)
        {
            var entities = await _repo.GetAllAsync(ct);
            return entities.Adapt<IEnumerable<NoteDto>>();
        }
    }
}
