using MediatR;
using Microsoft.Extensions.Logging;
using NotesApi.Application.Commands.CreateNote;
using NotesApi.Application.Common.Exceptions;
using NotesApi.Application.Repository;

namespace NotesApi.Application.Commands.DeleteNote
{
    public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand, bool>
    {
        private readonly ILogger<DeleteNoteCommandHandler> _logger;
        private readonly INoteRepository _repo;

        public DeleteNoteCommandHandler(
            ILogger<DeleteNoteCommandHandler> logger,
            INoteRepository repo)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteNoteCommand command, CancellationToken ct = default)
        {
            if (await _repo.DeleteAsync(command.Id, ct))
            {
                _logger.LogInformation($"Deleted note {command.Id}");
                return true;
            }

            throw new NotFoundException($"Note with ID:{command.Id} was not found");
        }
    }
}
