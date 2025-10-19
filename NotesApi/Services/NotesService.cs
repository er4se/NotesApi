using NotesApi.Data;
using NotesApi.Models;

namespace NotesApi.Services
{
    public class NotesService : INotesService
    {
        private readonly AppDbContext _db;
        public NotesService(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Получение всех заметок в формате списка
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Note> GetAll() => _db.Notes.ToList();

        /// <summary>
        /// Получение заметки по её идентификатору
        /// </summary>
        /// <param name="id">Идентификатор заметки (int)</param>
        /// <returns></returns>
        public Note? GetById(int id) => _db.Notes.Find(id);

        /// <summary>
        /// Создание новой заметки
        /// </summary>
        /// <param name="note">Экземпляр создаваемой заметки (Note)</param>
        /// <returns></returns>
        public Note Create(Note note)
        {
            _db.Notes.Add(note);
            _db.SaveChanges();

            return note;
        }

        /// <summary>
        /// Обновление данных существующей заметки
        /// </summary>
        /// <param name="id">Идентификатор заметки (int)</param>
        /// <param name="updatedNote">Экземпляр обновленной заметки (Note)</param>
        /// <returns></returns>
        public bool Update(int id, Note updatedNote)
        {
            var existing = _db.Notes.Find(id);
            if (existing == null) return false;

            existing.Title = updatedNote.Title;
            existing.Content = updatedNote.Content;

            _db.SaveChanges();

            return true;
        }

        /// <summary>
        /// Удаление существующей заметки
        /// </summary>
        /// <param name="id">Идентификатор заметки (int)</param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            var existing = _db.Notes.Find(id);
            if (existing == null) return false;

            _db.Notes.Remove(existing);
            _db.SaveChanges();

            return true;
        }
    }
}
