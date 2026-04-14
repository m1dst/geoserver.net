using Newtonsoft.Json;

namespace GeoServer.Models.Stores;

/// <summary>
/// Single coverage store response wrapper.
/// </summary>
public sealed class CoverageStoreResponse
{
    /// <summary>
    /// Coverage store object.
    /// </summary>
    [JsonProperty("coverageStore")]
    public CoverageStoreDto CoverageStore { get; set; } = new();
}
