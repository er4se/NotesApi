using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Contracts.Events.Abstractions
{
    /// <summary>
    /// Base event contract: note deleted (V1)
    /// </summary>
    public interface INoteDeleted
    {
        Guid NoteId { get; }
        DateTime DeletedAt { get; }
    }
}
