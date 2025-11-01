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
    public class UpdateNoteCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }

    public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand, bool>
    {
        private readonly INoteRepository _repo;
        public UpdateNoteCommandHandler(INoteRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(UpdateNoteCommand command, CancellationToken ct = default)
        {
            var entity = new Note { Id = command.Id, Title = command.Title, Content = command.Content };
            if (await _repo.UpdateAsync(entity.Id, entity, ct))
                return true;

            return false;
        }
    }
}
