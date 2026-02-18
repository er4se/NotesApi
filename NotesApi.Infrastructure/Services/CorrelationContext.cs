using Microsoft.AspNetCore.Http;
using NotesApi.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Infrastructure.Services
{
    public class CorrelationContext : ICorrelationContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CorrelationContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid CorrelationId
        {
            get
            {
                var correlationId = _httpContextAccessor.HttpContext?.Items["CorrelationId"]?.ToString();

                if(String.IsNullOrEmpty(correlationId)) 
                    correlationId = Guid.NewGuid().ToString();

                return Guid.Parse(correlationId);
            }
        }
    }
}
