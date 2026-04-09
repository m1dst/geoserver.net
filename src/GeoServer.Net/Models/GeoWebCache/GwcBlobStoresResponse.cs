using Newtonsoft.Json.Linq;

namespace geoserver.net.Models.GeoWebCache;

/// <summary>
/// GeoWebCache blob stores response wrapper.
/// </summary>
public sealed class GwcBlobStoresResponse
{
    /// <summary>
    /// Raw blob stores payload.
    /// </summary>
    [Newtonsoft.Json.JsonExtensionData]
    public System.Collections.Generic.IDictionary<string, JToken> Payload { get; set; } = new System.Collections.Generic.Dictionary<string, JToken>();
}
