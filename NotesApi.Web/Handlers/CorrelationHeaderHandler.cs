using NotesApi.Application.Common.Context;

namespace NotesApi.Web.Handlers
{
    public class CorrelationHeaderHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
        {
            var correlation = AsyncStorage<Correlation>.Retrieve();
            if (correlation is not null)
            {
                request.Headers.Add("CorrelationId", correlation.Id.ToString());
            }

            return await base.SendAsync(request, ct);
        }
    }
}
