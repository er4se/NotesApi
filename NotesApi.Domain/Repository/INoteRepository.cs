using NotesApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Application.Repository
{
    public interface INoteRepository
    {
        //IEnumerable<Note> GetAll();
        //Note? GetById(int id);
        Task<IEnumerable<Note>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Note?> GetByIdAsync(int id, CancellationToken cancellationToken = default); 
        Task<Note> CreateAsync(Note note, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(int id, Note note, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        //bool Update(int id, Note note);
        //bool Delete(int id);
    }
}
