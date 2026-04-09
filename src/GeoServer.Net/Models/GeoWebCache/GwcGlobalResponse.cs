using Newtonsoft.Json;

namespace geoserver.net.Models.GeoWebCache;

/// <summary>
/// GeoWebCache global configuration response.
/// </summary>
public sealed class GwcGlobalResponse
{
    /// <summary>
    /// Global configuration payload.
    /// </summary>
    [JsonProperty("global")]
    public object? Global { get; set; }
}
