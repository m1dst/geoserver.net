using Newtonsoft.Json;

namespace geoserver.net.Models.StructuredCoverages;

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
