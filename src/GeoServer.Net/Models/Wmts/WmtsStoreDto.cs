using Newtonsoft.Json;

namespace geoserver.net.Models.Wmts;

/// <summary>
/// WMTS store representation.
/// </summary>
public sealed class WmtsStoreDto
{
    /// <summary>
    /// Store name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
}
