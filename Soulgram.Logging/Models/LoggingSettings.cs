using Serilog.Events;
using Soulgram.Logging.Enums;

namespace Soulgram.Logging.Models;

public record LoggingSettings
{
    public string AppName { get; init; } = null!;
    public LogEventLevel MinimumLevel { get; init; } = LogEventLevel.Information;
    public LogSource[] LogSources { get; init; } = {LogSource.Console};
    public AvailableEnrichers[]? Enrichers { get; init; }
    public IReadOnlyDictionary<string, LogEventLevel>? SourceContextToExclude { get; init; }
}