# Soulgram Logging

Nuget package with soulgram logging functionallity

## Installation

Need to register logging via Asp Net Core DI

```csharp
 public static IServiceCollection AddLogging(
        this IServiceCollection services,
        LoggingSettings settings){}
```

Also need use Serilog on app builder
```csharp
 builder.UseSerilog();
```
