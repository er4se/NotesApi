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
        IEnumerable<Note> GetAll();
        Note? GetById(int id);
        Note Create(Note note);
        bool Update(int id, Note note);
        bool Delete(int id);
    }
}
