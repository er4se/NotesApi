using MassTransit;
using Microsoft.Extensions.Logging;
using NotesApi.Application.Common.Context;
using NotesApi.Contracts.Events.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Infrastructure.Consumers
{
    public class NoteCreatedConsumer : IConsumer<NoteCreated>
    {
        private readonly ILogger<NoteCreatedConsumer> _logger;

        public NoteCreatedConsumer(ILogger<NoteCreatedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<NoteCreated> context)
        {
            _logger.LogInformation(
            "Consumed NoteCreated event. MessageId={MessageId}, NoteId={NoteId}, MT-CorrelationId={CorrelationId}",
            context.MessageId,
            context.Message.NoteId,
            context.CorrelationId); // Это из MassTransit context

            // Лог 2: Что в AsyncStorage
            var correlation = AsyncStorage<Correlation>.Retrieve();
            _logger.LogInformation(
                "AsyncStorage CorrelationId: {AsyncStorageCorrelationId}",
                correlation?.Id.ToString() ?? "NULL");

            //var message = context.Message;
            //var messageId = context.MessageId;
            //
            //_logger.LogInformation(
            //    "Consumed NoteCreated event. MessageId={MessageId}, NoteId={NoteId}",
            //    messageId,
            //    message.NoteId);

            return Task.CompletedTask;
        }
    }
}
