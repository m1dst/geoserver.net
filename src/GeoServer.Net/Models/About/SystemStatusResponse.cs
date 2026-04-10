using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace geoserver.net.Models.About;

/// <summary>
/// Typed response for <c>about/system-status</c> metrics.
/// </summary>
public sealed class SystemStatusResponse
{
    /// <summary>
    /// Metrics envelope.
    /// </summary>
    [JsonProperty("metrics")]
    public SystemStatusMetricsEnvelope Metrics { get; set; } = new();

    /// <summary>
    /// Backward-compatible alias for payloads that expose metric array at root.
    /// </summary>
    [JsonProperty("metric")]
    private List<SystemStatusMetricDto>? RootMetricAlias
    {
        set
        {
            if (value is not null && value.Count > 0)
            {
                Metrics.Metric = value;
            }
        }
    }
}

/// <summary>
/// Envelope for system status metrics.
/// </summary>
public sealed class SystemStatusMetricsEnvelope
{
    /// <summary>
    /// Metrics collection.
    /// </summary>
    [JsonProperty("metric")]
    public List<SystemStatusMetricDto> Metric { get; set; } = new();

    /// <summary>
    /// Backward-compatible alias for payloads using <c>metrics</c>.
    /// </summary>
    [JsonProperty("metrics")]
    private List<SystemStatusMetricDto>? MetricsAlias
    {
        set
        {
            if (value is not null && value.Count > 0)
            {
                Metric = value;
            }
        }
    }
}

/// <summary>
/// Single system metric entry.
/// </summary>
public sealed class SystemStatusMetricDto
{
    /// <summary>
    /// Availability flag.
    /// </summary>
    [JsonProperty("available")]
    public bool? Available { get; set; }

    /// <summary>
    /// Metric category.
    /// </summary>
    [JsonProperty("category")]
    public string? Category { get; set; }

    /// <summary>
    /// Metric description.
    /// </summary>
    [JsonProperty("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Resource identifier related to metric.
    /// </summary>
    [JsonProperty("identifier")]
    public string? Identifier { get; set; }

    /// <summary>
    /// Metric name.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Sort priority value.
    /// </summary>
    [JsonProperty("priority")]
    public int? Priority { get; set; }

    /// <summary>
    /// Metric unit.
    /// </summary>
    [JsonProperty("unit")]
    public string? Unit { get; set; }

    /// <summary>
    /// Metric value.
    /// </summary>
    [JsonProperty("value")]
    public JToken? Value { get; set; }

    /// <summary>
    /// Additional unknown metric properties.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, JToken> AdditionalData { get; set; } = new Dictionary<string, JToken>();
}
