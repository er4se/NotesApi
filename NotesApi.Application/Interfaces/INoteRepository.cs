using NotesApi.Domain.Models;

namespace NotesApi.Application.Interfaces
{
    public interface INoteRepository
    {
        Task<IEnumerable<Note>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Note?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default); 
        Task<Note> CreateAsync(Note note, CancellationToken cancellationToken = default);
        Task UpdateAsync(Note note, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
