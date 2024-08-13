namespace MaisQ1Dev.Libs.Domain.Tracing;

public static class AsyncStorage<T> where T : new()
{
    private static readonly AsyncLocal<T> _asyncLocal = new();

    public static T Store(T val)
    {
        _asyncLocal.Value = val;
        return _asyncLocal.Value;
    }

    public static T? Retrieve()
        => _asyncLocal.Value;
}