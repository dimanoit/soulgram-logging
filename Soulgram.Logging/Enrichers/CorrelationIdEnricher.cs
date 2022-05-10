using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace Soulgram.Logging;

public class CorrelationIdEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor _httpContextAccessor = new HttpContextAccessor();
    private readonly string CorrelationIdHeader = "X-Correlation-Id";
    private readonly string CorrelationIdProperty = "CorrelationId";
    
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        _httpContextAccessor.HttpContext?.Request?.Headers?
            .TryGetValue(CorrelationIdHeader, out var correlationId);

        if (string.IsNullOrEmpty(correlationId))
        {
            return;
        }

        var property = propertyFactory.CreateProperty(CorrelationIdProperty, correlationId);
        logEvent.AddOrUpdateProperty(property);
    }
}