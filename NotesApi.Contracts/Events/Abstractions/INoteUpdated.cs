using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Contracts.Events.Abstractions
{
    /// <summary>
    /// Base event contract: note updated (V1)
    /// </summary>
    public interface INoteUpdated
    {
        Guid NoteId { get; }
        string Title { get; }
        DateTime UpdatedAt { get; }
    }
}
