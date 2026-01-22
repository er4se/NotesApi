using MassTransit;
using Microsoft.Extensions.Logging;
using NotesApi.Application.Common.Context;
using Serilog.Context;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Infrastructure.Consumers.Filters
{
    public class CorrelationConsumeFilter<T> : IFilter<ConsumeContext<T>> where T : class
    {
        public Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
        {
            var correlationIdHeader = context.CorrelationId;
            if (correlationIdHeader.HasValue)
            {
                var correlationId = correlationIdHeader.Value;
                Serilog.Context.LogContext.PushProperty("CorrelationId", new ScalarValue(correlationId));
                AsyncStorage<Correlation>.Store(new Correlation
                {
                    Id = correlationId
                });
            }

            return next.Send(context);
        }

        public void Probe(ProbeContext context)
        {
            // context.CreateFilterScope("CorrelationIdConsumerFilter");
        }
    }
}
