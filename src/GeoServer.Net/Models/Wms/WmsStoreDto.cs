using Newtonsoft.Json;

namespace geoserver.net.Models.Wms;

/// <summary>
/// WMS store representation.
/// </summary>
public sealed class WmsStoreDto
{
    /// <summary>
    /// Store name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
}
