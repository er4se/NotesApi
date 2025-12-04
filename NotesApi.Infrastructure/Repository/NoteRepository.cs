using Microsoft.EntityFrameworkCore;
using NotesApi.Application.Interfaces;
using NotesApi.Domain.Models;
using NotesApi.Domain.Common.Exceptions;
using NotesApi.Infrastructure.Data;

namespace NotesApi.Infrastructure.Repository
{
    public class NoteRepository : INoteRepository
    {
        private readonly AppDbContext _db;
        public NoteRepository(AppDbContext db) => _db = db;

        /// <summary>
        /// Получение всех заметок в формате списка
        /// </summary>
        public async Task<IEnumerable<Note>> GetAllAsync(CancellationToken cancellationToken = default) 
            => await _db.Notes.ToListAsync(cancellationToken);

        /// <summary>
        /// Получение заметки по её идентификатору
        /// </summary>
        public async Task<Note?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) 
            => await _db.Notes.FindAsync(id, cancellationToken);

        /// <summary>
        /// Создание новой заметки
        /// </summary>
        public async Task<Note> CreateAsync(Note note, CancellationToken cancellationToken = default)
        {
            await _db.Notes.AddAsync(note, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            return note;
        }

        /// <summary>
        /// Обновление данных существующей заметки
        /// </summary>
        public async Task UpdateAsync(Note note, CancellationToken cancellationToken = default)
        {
            _db.Notes.Update(note);
            await _db.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Удаление существующей заметки
        /// </summary>
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var note = await _db.Notes.FindAsync(id) ??
                throw new NotFoundException($"NOTE entity with ID: [{id}] was not found");

            _db.Notes.Remove(note);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
