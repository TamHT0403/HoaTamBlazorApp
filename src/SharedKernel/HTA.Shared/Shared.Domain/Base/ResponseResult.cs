namespace Shared.Domain
{
    public record ResponseResult<T>(
        bool Success,
        T? Data,
        ErrorDetail? Error,
        DateTime Timestamp,
        string TraceId);

    public record ErrorDetail(
        string Code,
        string Message,
        Dictionary<string, string[]>? Details = null);
}