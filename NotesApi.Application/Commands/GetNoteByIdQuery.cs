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
    public class GetNoteByIdQuery : IRequest<NoteDto?>
    {
        public int Id { get; set; }
    }

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
