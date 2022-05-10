using Serilog;
using Serilog.Events;
using Soulgram.Logging.Enums;
using Soulgram.Logging.Models;

namespace Soulgram.Logging;

public static class LoggerConfigurationBuilder
{
    private static LoggerConfiguration SetMinimumLevel(this LoggerConfiguration configuration, LogEventLevel level)
    {
        return configuration.MinimumLevel.Is(level);
    }

    private static LoggerConfiguration ExcludeFromContext(
        this LoggerConfiguration configuration,
        IReadOnlyDictionary<string, LogEventLevel>? sourcesToExclude)
    {
        if (sourcesToExclude is null) return configuration;

        foreach (var keyValue in sourcesToExclude)
            configuration.Filter.ByExcludingLogsFrom(keyValue.Key, keyValue.Value);

        return configuration;
    }

    private static LoggerConfiguration AddEnrichers(
        this LoggerConfiguration configuration,
        params AvailableEnrichers[]? enrichers)
    {
        if (enrichers == null) return configuration;

        foreach (var enricher in enrichers)
            switch (enricher)
            {
                case AvailableEnrichers.UserClaim:
                    configuration.Enrich.With<UserClaimsEnricher>();
                    break;
                case AvailableEnrichers.FromLogContext:
                    configuration.Enrich.FromLogContext();
                    break;
            }

        return configuration;
    }

    private static LoggerConfiguration AddWriteToSources(
        this LoggerConfiguration configuration,
        LogSource[] sources,
        string? outputFormat)
    {
        ArgumentNullException.ThrowIfNull(sources);

        foreach (var logSource in sources)
            switch (logSource)
            {
                case LogSource.Console:
                {
                    if (string.IsNullOrEmpty(outputFormat))
                        configuration.WriteTo.Console();
                    else
                        configuration.WriteTo.Console(outputTemplate: outputFormat);
                    break;
                }
                case LogSource.Debug:
                {
                    if (string.IsNullOrEmpty(outputFormat))
                        configuration.WriteTo.Debug();
                    else
                        configuration.WriteTo.Debug(outputTemplate: outputFormat);
                    break;
                }
            }

        return configuration;
    }

    public static LoggerConfiguration GetConfiguration(LoggingSettings settings)
    {
        var configuration = new LoggerConfiguration()
            .AddWriteToSources(settings.LogSources, settings.OutputFormat)
            .SetMinimumLevel(settings.MinimumLevel)
            .AddEnrichers(settings.Enrichers)
            .ExcludeFromContext(settings.SourceContextToExclude);

        return configuration;
    }
}