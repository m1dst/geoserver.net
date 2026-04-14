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
    /// Display value.
    /// </summary>
    [JsonProperty("displayValue")]
    public string? DisplayValue
    {
        get
        {
            if (Value is null)
                return null;

            var raw = Value.Type == JTokenType.String
                ? Value.Value<string>()
                : Value.ToString(Formatting.None);

            if (string.IsNullOrWhiteSpace(Unit))
                return raw;

            if (raw.Equals("NOT AVAILABLE", StringComparison.OrdinalIgnoreCase))
                return raw;

            return MetricFormatters.TryGetValue(Unit, out var formatter)
                ? formatter(raw ?? string.Empty)
                : raw;
        }
    }

    /// <summary>
    /// Additional unknown metric properties.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, JToken> AdditionalData { get; set; } = new Dictionary<string, JToken>();

    private static readonly IReadOnlyDictionary<string, Func<string, string>> MetricFormatters =
        new Dictionary<string, Func<string, string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["%"] = v => double.TryParse(v.TrimEnd('%'), out var parsedValue) ? $"{parsedValue:n0} %" : v,
            ["bytes"] = v =>
            {
                if (long.TryParse(v, out var bytes))
                {
                    const long KB = 1024;
                    const long MB = KB * 1024;
                    const long GB = MB * 1024;
                    if (bytes >= GB)
                        return $"{bytes / (double)GB:n2} GB";
                    if (bytes >= MB)
                        return $"{bytes / (double)MB:n2} MB";
                    if (bytes >= KB)
                        return $"{bytes / (double)KB:n2} KB";
                    return $"{bytes} bytes";
                }
                return $"{v} bytes";
            },
            ["sec"] = v =>
            {
                if (double.TryParse(v, out var seconds))
                {
                    var timeSpan = TimeSpan.FromSeconds(seconds);
                    var parts = new List<string>();
                    if (timeSpan.Days > 0)
                        parts.Add($"{timeSpan.Days} days");
                    if (timeSpan.Hours > 0)
                        parts.Add($"{timeSpan.Hours} hours");
                    if (timeSpan.Minutes > 0)
                        parts.Add($"{timeSpan.Minutes} minutes");
                    if (timeSpan.Seconds > 0 || parts.Count == 0)
                        parts.Add($"{timeSpan.Seconds} seconds");
                    return string.Join(", ", parts);
                }
                return $"{v} seconds";
            }
        };

}
