using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Contracts.Events.Abstractions
{
    /// <summary>
    /// Base event contract: note created (V1)
    /// </summary>
    public interface INoteCreated
    {
        Guid NoteId { get; }
        string Title { get; }
        DateTime CreatedAt { get; }
    }
}
