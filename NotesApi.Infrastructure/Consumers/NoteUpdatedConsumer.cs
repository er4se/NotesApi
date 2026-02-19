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
    public class NoteUpdatedConsumer : IConsumer<NoteUpdated>
    {
        private readonly ILogger<NoteUpdatedConsumer> _logger;

        public NoteUpdatedConsumer(ILogger<NoteUpdatedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<NoteUpdated> context)
        {
            var message = context.Message;
            var messageId = context.MessageId;

            using (Serilog.Context.LogContext.PushProperty("CorrelationId", message.CorrelationId))
            {
                _logger.LogInformation(
                "Consumed NoteUpdated event. MessageId={MessageId}, NoteId={NoteId}",
                messageId,
                message.NoteId);
            }

            return Task.CompletedTask;
        }
    }
}
