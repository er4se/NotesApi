using MassTransit;
using Microsoft.Extensions.Logging;
using NotesApi.Contracts.Events.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Infrastructure.Consumers
{
    public class NoteDeletedConsumer : IConsumer<NoteDeleted>
    {
        private readonly ILogger<NoteDeletedConsumer> _logger;

        public NoteDeletedConsumer(ILogger<NoteDeletedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<NoteDeleted> context)
        {
            var message = context.Message;
            var messageId = context.MessageId;

            using (Serilog.Context.LogContext.PushProperty("CorrelationId", message.CorrelationId))
            {
                _logger.LogInformation(
                "Consumed NoteDeleted event. MessageId={MessageId}, NoteId={NoteId}",
                messageId,
                message.NoteId);
            }

            return Task.CompletedTask;
        }
    }
}
