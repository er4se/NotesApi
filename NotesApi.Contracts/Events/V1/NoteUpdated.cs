using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Contracts.Events.V1
{
    /// <summary>
    /// Event: note updated (V1)
    /// </summary>
    public record NoteUpdated : CorrelatedBy<Guid>
    {
        public Guid NoteId { get; init; }
        public string Title { get; init; } = string.Empty;
        public DateTime UpdatedAt { get; init; }

        public Guid CorrelationId { get; init; }
    }
}
