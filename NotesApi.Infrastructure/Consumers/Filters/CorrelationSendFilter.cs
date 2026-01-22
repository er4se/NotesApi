using MassTransit;
using NotesApi.Application.Common.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Infrastructure.Consumers.Filters
{
    public class CorrelationSendFilter<T> : IFilter<SendContext<T>> where T : class
    {
        public Task Send(SendContext<T> context, IPipe<SendContext<T>> next)
        {
            var correlation = AsyncStorage<Correlation>.Retrieve();
            if (correlation is not null)
            {
                context.CorrelationId = correlation.Id;
            }

            return next.Send(context);
        }

        public void Probe(ProbeContext context)
        {
        }
    }
}
