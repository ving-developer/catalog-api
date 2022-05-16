using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace Catalog_API.Logging;

public class CustomLoggerProvider : ILoggerProvider
{
    readonly CustomLoggerProviderConfiguration loggerConfiguration;
    readonly ConcurrentDictionary<string, CustomLogger> loggers =
        new ConcurrentDictionary<string, CustomLogger>();

    public CustomLoggerProvider(IOptionsMonitor<CustomLoggerProviderConfiguration> configuration)
    {
        loggerConfiguration = configuration.CurrentValue;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return loggers.GetOrAdd(categoryName, new CustomLogger(loggerConfiguration));
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
