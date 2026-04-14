using Newtonsoft.Json;

namespace GeoServer.Models.Stores;

/// <summary>
/// Coverage store representation.
/// </summary>
public sealed class CoverageStoreDto
{
    /// <summary>
    /// Coverage store name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Coverage store type.
    /// </summary>
    [JsonProperty("type")]
    public string? Type { get; set; }
}
