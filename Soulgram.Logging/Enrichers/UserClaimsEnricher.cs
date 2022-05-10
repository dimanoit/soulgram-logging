using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace Soulgram.Logging;

internal class UserClaimsEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor _httpContextAccessor = new HttpContextAccessor();
    private string UserLogProperty => "User";

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var claims = _httpContextAccessor.HttpContext?.User?.Claims.ToArray();

        if (claims == null || !claims.Any()) return;

        var userId = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        var userEmail = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;

        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty(UserLogProperty, new
        {
            UserId = userId,
            Email = userEmail
        }, true));
    }
}