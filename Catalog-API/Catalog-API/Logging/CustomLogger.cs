namespace Catalog_API.Logging;

public class CustomLogger : ILogger
{
    readonly CustomLoggerProviderConfiguration customLoggerProviderConfig;

    public CustomLogger(CustomLoggerProviderConfiguration customLoggerProviderConfiguration)
    {
        this.customLoggerProviderConfig = customLoggerProviderConfiguration;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel == customLoggerProviderConfig.LogLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        //Configuring log string
        string message = $"{logLevel}: {eventId} - {formatter(state, exception)}";
        WriteLogs(message);
    }

    private void WriteLogs(string message)
    {
        //Setting the path to the log file
        var filePath = $@"{Directory.GetCurrentDirectory()}\..\log.txt";
        //Writing log string to file
        using StreamWriter streamWriter = new StreamWriter(filePath, true);
        try
        {
            streamWriter.WriteLine(message);
            streamWriter.Close();
        }
        catch (Exception)
        {
            throw;
        }
    }
}