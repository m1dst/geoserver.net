using Newtonsoft.Json;

namespace GeoServer.Models.Coverages;

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
