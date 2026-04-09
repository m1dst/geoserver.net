using Newtonsoft.Json.Linq;

namespace geoserver.net.Models.GeoWebCache;

/// <summary>
/// GeoWebCache disk quota response wrapper.
/// </summary>
public sealed class GwcDiskQuotaResponse
{
    /// <summary>
    /// Raw disk quota payload.
    /// </summary>
    [Newtonsoft.Json.JsonExtensionData]
    public System.Collections.Generic.IDictionary<string, JToken> Payload { get; set; } = new System.Collections.Generic.Dictionary<string, JToken>();
}
