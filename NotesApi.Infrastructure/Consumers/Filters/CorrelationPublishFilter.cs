using MassTransit;
using NotesApi.Application.Common.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Infrastructure.Consumers.Filters
{
    public class CorrelationPublishFilter<T> : IFilter<PublishContext<T>> where T : class
    {
        public Task Send(PublishContext<T> context, IPipe<PublishContext<T>> next)
        {
            var correlation = AsyncStorage<Correlation>.Retrieve();
            if (correlation is not null)
            {
                context.CorrelationId = correlation.Id;

                Console.WriteLine($"[PUBLISH FILTER] Setting CorrelationId: {correlation.Id}");
            }

            return next.Send(context);
        }

        public void Probe(ProbeContext context) { }
    }
}
