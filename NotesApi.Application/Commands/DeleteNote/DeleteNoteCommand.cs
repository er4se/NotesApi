using MediatR;

namespace NotesApi.Application.Commands.DeleteNote
{
    public class DeleteNoteCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
