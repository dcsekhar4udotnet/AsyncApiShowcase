namespace ApiFrontend.Models;

public sealed record ServiceDataResponse(
    string Source,
    string Message,
    string? Input,
    int DelayMs,
    DateTimeOffset Timestamp);
