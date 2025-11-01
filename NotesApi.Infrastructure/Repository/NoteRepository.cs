using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NotesApi.Application.Repository;
using NotesApi.Domain.Models;
using NotesApi.Infrastructure.Data;

namespace NotesApi.Infrastructure.Repository
{
    public class NoteRepository : INoteRepository
    {
        private readonly AppDbContext _db;
        public NoteRepository(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Получение всех заметок в формате списка
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Note>> GetAllAsync(CancellationToken cancellationToken = default) => await _db.Notes.ToListAsync(cancellationToken);

        /// <summary>
        /// Получение заметки по её идентификатору
        /// </summary>
        /// <param name="id">Идентификатор заметки (int)</param>
        /// <returns></returns>
        public async Task<Note?> GetByIdAsync(int id, CancellationToken cancellationToken = default) => await _db.Notes.FindAsync(id, cancellationToken);

        /// <summary>
        /// Создание новой заметки
        /// </summary>
        /// <param name="note">Экземпляр создаваемой заметки (Note)</param>
        /// <returns></returns>
        public async Task<Note> CreateAsync(Note entity, CancellationToken cancellationToken = default)
        {
            await _db.Notes.AddAsync(entity, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            return entity;
        }

        /// <summary>
        /// Обновление данных существующей заметки
        /// </summary>
        /// <param name="id">Идентификатор заметки (int)</param>
        /// <param name="updatedNote">Экземпляр обновленной заметки (Note)</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(int id, Note entity, CancellationToken cancellationToken = default)
        {
            var existing = await _db.Notes.FindAsync(id, cancellationToken);
            if (existing == null) return false;

            existing.Title = entity.Title;
            existing.Content = entity.Content;

            await _db.SaveChangesAsync(cancellationToken);

            return true;
        }

        /// <summary>
        /// Удаление существующей заметки
        /// </summary>
        /// <param name="id">Идентификатор заметки (int)</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var existing = await _db.Notes.FindAsync(id, cancellationToken);
            if (existing == null) return false;

            _db.Notes.Remove(existing);
            await _db.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
