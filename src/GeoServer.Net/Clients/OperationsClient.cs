using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using geoserver.net.Models.Operations;

namespace geoserver.net.Clients;

/// <summary>
/// Wrapper around reload, reset, logging and monitoring operations.
/// </summary>
public sealed class OperationsClient : GeoServerClientBase
{
    internal OperationsClient(HttpClient httpClient) : base(httpClient)
    {
    }

    /// <summary>
    /// Resets internal caches.
    /// </summary>
    public Task ResetAsync(CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, "reset", cancellationToken: cancellationToken);

    /// <summary>
    /// Resets internal caches (synchronous).
    /// </summary>
    public void Reset() => Send(HttpMethod.Post, "reset");

    /// <summary>
    /// Reloads configuration from disk and resets caches.
    /// </summary>
    public Task ReloadAsync(CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, "reload", cancellationToken: cancellationToken);

    /// <summary>
    /// Reloads configuration from disk and resets caches (synchronous).
    /// </summary>
    public void Reload() => Send(HttpMethod.Post, "reload");

    /// <summary>
    /// Gets logging configuration.
    /// </summary>
    public Task<LoggingResponse> GetLoggingAsync(CancellationToken cancellationToken = default)
        => SendAsync<LoggingResponse>(HttpMethod.Get, "logging.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets logging configuration (synchronous).
    /// </summary>
    public LoggingResponse GetLogging() => Send<LoggingResponse>(HttpMethod.Get, "logging.json");

    /// <summary>
    /// Updates logging configuration.
    /// </summary>
    public Task UpdateLoggingAsync(object loggingPayload, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, "logging", loggingPayload, cancellationToken);

    /// <summary>
    /// Updates logging configuration (synchronous).
    /// </summary>
    public void UpdateLogging(object loggingPayload)
        => Send(HttpMethod.Put, "logging", loggingPayload);

    /// <summary>
    /// Gets monitoring requests as raw payload (CSV/JSON/HTML depending on query and server configuration).
    /// </summary>
    public Task<string> GetMonitoringRequestsRawAsync(string? query = null, CancellationToken cancellationToken = default)
        => SendRawAsync(HttpMethod.Get, BuildMonitorPath("monitor/requests", query), cancellationToken: cancellationToken);

    /// <summary>
    /// Gets monitoring requests as raw payload (synchronous).
    /// </summary>
    public string GetMonitoringRequestsRaw(string? query = null)
        => SendRaw(HttpMethod.Get, BuildMonitorPath("monitor/requests", query));

    /// <summary>
    /// Gets monitoring requests as typed JSON payload.
    /// </summary>
    public Task<MonitoringRequestsResponse> GetMonitoringRequestsTypedAsync(string? query = null, CancellationToken cancellationToken = default)
        => SendAsync<MonitoringRequestsResponse>(HttpMethod.Get, BuildMonitorPath("monitor/requests.json", query), cancellationToken: cancellationToken);

    /// <summary>
    /// Gets monitoring requests as typed JSON payload (synchronous).
    /// </summary>
    public MonitoringRequestsResponse GetMonitoringRequestsTyped(string? query = null)
        => Send<MonitoringRequestsResponse>(HttpMethod.Get, BuildMonitorPath("monitor/requests.json", query));

    /// <summary>
    /// Gets a monitoring request record by id as raw payload.
    /// </summary>
    public Task<string> GetMonitoringRequestRawAsync(string requestId, string? query = null, CancellationToken cancellationToken = default)
        => SendRawAsync(HttpMethod.Get, BuildMonitorPath($"monitor/requests/{requestId}", query), cancellationToken: cancellationToken);

    /// <summary>
    /// Gets a monitoring request record by id as raw payload (synchronous).
    /// </summary>
    public string GetMonitoringRequestRaw(string requestId, string? query = null)
        => SendRaw(HttpMethod.Get, BuildMonitorPath($"monitor/requests/{requestId}", query));

    /// <summary>
    /// Gets a monitoring request record by id as typed JSON payload.
    /// </summary>
    public Task<MonitoringRequestResponse> GetMonitoringRequestTypedAsync(string requestId, string? query = null, CancellationToken cancellationToken = default)
        => SendAsync<MonitoringRequestResponse>(HttpMethod.Get, BuildMonitorPath($"monitor/requests/{requestId}.json", query), cancellationToken: cancellationToken);

    /// <summary>
    /// Gets a monitoring request record by id as typed JSON payload (synchronous).
    /// </summary>
    public MonitoringRequestResponse GetMonitoringRequestTyped(string requestId, string? query = null)
        => Send<MonitoringRequestResponse>(HttpMethod.Get, BuildMonitorPath($"monitor/requests/{requestId}.json", query));

    /// <summary>
    /// Clears all monitoring requests.
    /// </summary>
    public Task ClearMonitoringRequestsAsync(CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, "monitor/requests", cancellationToken: cancellationToken);

    /// <summary>
    /// Clears all monitoring requests (synchronous).
    /// </summary>
    public void ClearMonitoringRequests()
        => Send(HttpMethod.Delete, "monitor/requests");

    private static string BuildMonitorPath(string basePath, string? query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return basePath;
        }

        return $"{basePath}?{query.TrimStart('?')}";
    }
}
