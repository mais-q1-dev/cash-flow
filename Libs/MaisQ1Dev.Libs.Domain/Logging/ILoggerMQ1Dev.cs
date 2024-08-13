namespace MaisQ1Dev.Libs.Domain.Logging;

public interface ILoggerMQ1Dev<T> where T : class
{
    void LogInformation(string message, params object?[] args);
    void LogWarning(string message, params object?[] args);
    void LogError(string message, params object?[] args);
    void LogError(Exception exception, string message, params object?[] args);
}
