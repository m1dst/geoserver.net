using Newtonsoft.Json;

namespace GeoServer.Models.StructuredCoverages;

/// <summary>
/// Structured coverage index schema response.
/// </summary>
public sealed class StructuredCoverageIndexResponse
{
    /// <summary>
    /// Raw schema payload from GeoServer.
    /// </summary>
    [JsonProperty("Schema")]
    public object? Schema { get; set; }
}
