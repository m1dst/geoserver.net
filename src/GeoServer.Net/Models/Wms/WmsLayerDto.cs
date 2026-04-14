using Newtonsoft.Json;

namespace GeoServer.Models.Wms;

/// <summary>
/// WMS layer representation.
/// </summary>
public sealed class WmsLayerDto
{
    /// <summary>
    /// Layer name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
}
