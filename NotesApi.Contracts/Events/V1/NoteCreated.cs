using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Contracts.Events.V1
{
    /// <summary>
    /// Event: note created (V1)
    /// </summary>
    public record NoteCreated
    {
        public Guid NoteId { get; init; }
        public string Title { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
    }
}
