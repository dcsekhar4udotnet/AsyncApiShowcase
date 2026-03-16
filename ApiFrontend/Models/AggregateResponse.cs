namespace ApiFrontend.Models;

public sealed record AggregateResponse(
    ServiceDataResponse Service1,
    ServiceDataResponse Service2,
    DateTimeOffset Timestamp);
