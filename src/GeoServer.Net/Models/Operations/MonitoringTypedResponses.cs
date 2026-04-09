using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace geoserver.net.Models.Operations;

/// <summary>
/// Typed response for monitoring requests list.
/// </summary>
public sealed class MonitoringRequestsResponse
{
    [JsonProperty("requests")]
    public List<MonitoringRequestDto> Requests { get; set; } = new();
}

/// <summary>
/// Typed response for a single monitoring request.
/// </summary>
public sealed class MonitoringRequestResponse
{
    [JsonProperty("request")]
    public MonitoringRequestDto Request { get; set; } = new();
}

/// <summary>
/// Monitoring request DTO.
/// </summary>
public sealed class MonitoringRequestDto
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("path")]
    public string? Path { get; set; }

    [JsonProperty("status")]
    public int? Status { get; set; }

    [JsonProperty("responseStatus")]
    public int? ResponseStatus { get; set; }

    [JsonProperty("method")]
    public string? Method { get; set; }

    [JsonProperty("startTime")]
    public string? StartTime { get; set; }

    [JsonProperty("endTime")]
    public string? EndTime { get; set; }

    [JsonProperty("remoteAddr")]
    public string? RemoteAddress { get; set; }

    [JsonProperty("remoteHost")]
    public string? RemoteHost { get; set; }

    [JsonProperty("responseLength")]
    public long? ResponseLength { get; set; }

    [JsonExtensionData]
    public IDictionary<string, JToken> AdditionalData { get; set; } = new Dictionary<string, JToken>();
}
