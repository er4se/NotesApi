using MediatR;
using Microsoft.Extensions.Logging;
using NotesApi.Application.Common.Exceptions;
using NotesApi.Application.Repository;
using NotesApi.Domain.Models;

namespace NotesApi.Application.Commands.UpdateNote
{
    public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand, bool>
    {
        private readonly ILogger<UpdateNoteCommandHandler> _logger;
        private readonly INoteRepository _repo;
        
        public UpdateNoteCommandHandler(
            ILogger<UpdateNoteCommandHandler> logger,
            INoteRepository repo)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateNoteCommand command, CancellationToken ct = default)
        {
            var entity = new Note { Id = command.Id, Title = command.Title, Content = command.Content };
            if (await _repo.UpdateAsync(entity.Id, entity, ct))
            {
                _logger.LogInformation($"Updated note {entity.Id}");
                return true;
            }

            throw new NotFoundException($"Note with ID:{command.Id} was not found");
        }
    }
}
