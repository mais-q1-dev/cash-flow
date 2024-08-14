using MaisQ1Dev.Libs.Domain.Tracing;
using Microsoft.Extensions.Logging;
using System.Text;

namespace MaisQ1Dev.Libs.Domain.Logging;

public class LoggerMQ1Dev<T> : ILoggerMQ1Dev<T>
    where T : class
{
    private ILogger Logger { get; }

    public LoggerMQ1Dev(ILoggerFactory loggerFactory)
        => Logger = loggerFactory.CreateLogger<T>();

    public void LogInformation(string message, params object?[] args)
    {
        if (Logger.IsEnabled(LogLevel.Information))
        {
            var correlation = AsyncStorage<Correlation>.Retrieve();

            var messageLog = new StringBuilder();
            messageLog.Append($"[CorrelationId:{correlation?.Id}] ");
            messageLog.Append(message);

            Logger.LogInformation(messageLog.ToString(), args);
            messageLog.Clear();
        }
    }

    public void LogWarning(string message, params object?[] args)
    {
        if (Logger.IsEnabled(LogLevel.Warning))
        {
            var correlation = AsyncStorage<Correlation>.Retrieve();

            var messageLog = new StringBuilder();
            messageLog.Append($"[CorrelationId:{correlation?.Id}] ");
            messageLog.Append(message);

            Logger.LogWarning(messageLog.ToString(), args);
            messageLog.Clear();
        }
    }

    public void LogError(string message, params object?[] args)
    {
        if (Logger.IsEnabled(LogLevel.Error))
        {
            var correlation = AsyncStorage<Correlation>.Retrieve();

            var messageLog = new StringBuilder();
            messageLog.Append($"[CorrelationId:{correlation?.Id}] ");
            messageLog.Append(message);

            Logger.LogError(messageLog.ToString(), args);
            messageLog.Clear();
        }
    }

    public void LogError(Exception exception, string message, params object?[] args)
    {
        if (Logger.IsEnabled(LogLevel.Error))
        {
            var correlation = AsyncStorage<Correlation>.Retrieve();

            var messageLog = new StringBuilder();
            messageLog.Append($"[CorrelationId:{correlation?.Id}] ");
            messageLog.Append(message);
            messageLog.Append($" [Exception: {exception.Message}]");
            if (exception.InnerException != null)
                messageLog.Append($" [InnerException: {exception.InnerException.Message}]");

            Logger.LogError(exception, messageLog.ToString(), args);
            messageLog.Clear();
        }
    }
}
