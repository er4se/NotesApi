using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Contracts.Events.V1
{
    /// <summary>
    /// Event: note deleted (V1)
    /// </summary>
    public record NoteDeleted
    {
        public Guid NoteId { get; init; }
        public DateTime DeletedAt { get; init; }
    }
}
