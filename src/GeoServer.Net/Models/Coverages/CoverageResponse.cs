using Newtonsoft.Json;

namespace geoserver.net.Models.Coverages;

/// <summary>
/// Single coverage response wrapper.
/// </summary>
public sealed class CoverageResponse
{
    /// <summary>
    /// Coverage object.
    /// </summary>
    [JsonProperty("coverage")]
    public CoverageDto Coverage { get; set; } = new();
}
