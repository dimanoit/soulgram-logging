using Serilog.Configuration;
using Serilog.Events;
using Serilog.Filters;

namespace Soulgram.Logging;

public static class LoggerConfigurationExtension
{
    public static void ByExcludingLogsFrom(
        this LoggerFilterConfiguration filterConfiguration,
        string sourceContext,
        LogEventLevel logLevel)
    {
        filterConfiguration.ByExcluding(LogsFrom(sourceContext, logLevel));
    }

    private static Func<LogEvent, bool> LogsFrom(string sourceContext, LogEventLevel level)
    {
        var sourceContextFilter = Matching.FromSource(sourceContext);
        return logEvent => logEvent.Level == level && sourceContextFilter(logEvent);
    }
}