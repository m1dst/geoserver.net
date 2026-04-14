using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GeoServer.Models.Operations;

/// <summary>
/// Typed response for monitoring requests list.
/// </summary>
public sealed class MonitoringRequestsResponse
{
    /// <summary>
    /// Monitoring request entries.
    /// </summary>
    [JsonProperty("requests")]
    public List<MonitoringRequestDto> Requests { get; set; } = new();
}

/// <summary>
/// Typed response for a single monitoring request.
/// </summary>
public sealed class MonitoringRequestResponse
{
    /// <summary>
    /// Single monitoring request entry.
    /// </summary>
    [JsonProperty("request")]
    public MonitoringRequestDto Request { get; set; } = new();
}

/// <summary>
/// Monitoring request DTO.
/// </summary>
public sealed class MonitoringRequestDto
{
    /// <summary>
    /// Request identifier.
    /// </summary>
    [JsonProperty("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Requested endpoint path.
    /// </summary>
    [JsonProperty("path")]
    public string? Path { get; set; }

    /// <summary>
    /// HTTP status (legacy field on some versions).
    /// </summary>
    [JsonProperty("status")]
    public int? Status { get; set; }

    /// <summary>
    /// HTTP response status.
    /// </summary>
    [JsonProperty("responseStatus")]
    public int? ResponseStatus { get; set; }

    /// <summary>
    /// HTTP method.
    /// </summary>
    [JsonProperty("method")]
    public string? Method { get; set; }

    /// <summary>
    /// Gets or sets the StartTime value.
    /// </summary>
    [JsonProperty("startTime")]
    public string? StartTime { get; set; }

    /// <summary>
    /// Gets or sets the EndTime value.
    /// </summary>
    [JsonProperty("endTime")]
    public string? EndTime { get; set; }

    /// <summary>
    /// Gets or sets the RemoteAddress value.
    /// </summary>
    [JsonProperty("remoteAddr")]
    public string? RemoteAddress { get; set; }

    /// <summary>
    /// Gets or sets the RemoteHost value.
    /// </summary>
    [JsonProperty("remoteHost")]
    public string? RemoteHost { get; set; }

    /// <summary>
    /// Gets or sets the ResponseLength value.
    /// </summary>
    [JsonProperty("responseLength")]
    public long? ResponseLength { get; set; }

    /// <summary>
    /// Gets or sets the AdditionalData value.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, JToken> AdditionalData { get; set; } = new Dictionary<string, JToken>();
}
