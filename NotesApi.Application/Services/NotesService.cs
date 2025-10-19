using NotesApi.Application.Repository;
using NotesApi.Domain.Models;

namespace NotesApi.Application.Services
{
    public class NotesService : INotesService
    {
        private readonly INoteRepository _repo;
        public NotesService(INoteRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<Note> GetAll() => _repo.GetAll();
        public Note? GetById(int id) => _repo.GetById(id);
        public Note Create(Note note) => _repo.Create(note);
        public bool Update(int id, Note updatedNote) => _repo.Update(id, updatedNote);
        public bool Delete(int id) => _repo.Delete(id);
    }
}
