using MediatR;
using NotesApi.Application.Repository;
using NotesApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Application.Commands
{
    public class CreateNoteCommand : IRequest<int>
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }

    public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, int>
    {
        private readonly INoteRepository _repo;
        public CreateNoteCommandHandler(INoteRepository repo)
        {
            _repo = repo;
        }

        public async Task<int> Handle(CreateNoteCommand command, CancellationToken ct = default)
        {
            var entity = new Note { Title = command.Title, Content = command.Content, CreatedAt = DateTime.UtcNow};
            await _repo.CreateAsync(entity, ct);
            return entity.Id;
        }
    }
}
