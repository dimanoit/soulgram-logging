using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Soulgram.Logging.Models;

namespace Soulgram.Logging;

public static class ServiceInjector
{
    public static IServiceCollection AddLogging(
        this IServiceCollection services,
        LoggingSettings settings)
    {
        Log.Logger = LoggerConfigurationBuilder
            .GetConfiguration(settings)
            .CreateLogger();

        return services;
    }
}