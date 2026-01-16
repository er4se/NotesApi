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
            var correlationId = context.CorrelationId;

            _logger.LogInformation(
                "Consumed NoteUpdated event. CorrelationId={CorrelationId}, MessageId={MessageId}, NoteId={NoteId}",
                correlationId,
                messageId,
                message.NoteId);

            return Task.CompletedTask;
        }
    }
}
