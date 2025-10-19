using NotesApi.Models;

namespace NotesApi.Services
{
    public interface INotesService
    {
        IEnumerable<Note> GetAll();
        Note? GetById(int id);
        Note Create(Note note);
        bool Update(int id, Note updatedNote);
        bool Delete(int id);
    }
}
