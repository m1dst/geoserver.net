using Newtonsoft.Json.Linq;

namespace GeoServer.Models.GeoWebCache;

/// <summary>
/// GeoWebCache layers response wrapper.
/// </summary>
public sealed class GwcLayersResponse
{
    /// <summary>
    /// Raw layers payload.
    /// </summary>
    [Newtonsoft.Json.JsonExtensionData]
    public System.Collections.Generic.IDictionary<string, JToken> Payload { get; set; } = new System.Collections.Generic.Dictionary<string, JToken>();
}
