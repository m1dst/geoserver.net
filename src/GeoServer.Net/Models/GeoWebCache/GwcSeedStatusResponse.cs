using Newtonsoft.Json.Linq;

namespace GeoServer.Models.GeoWebCache;

/// <summary>
/// GeoWebCache seed status response wrapper.
/// </summary>
public sealed class GwcSeedStatusResponse
{
    /// <summary>
    /// Raw seed status payload.
    /// </summary>
    [Newtonsoft.Json.JsonExtensionData]
    public System.Collections.Generic.IDictionary<string, JToken> Payload { get; set; } = new System.Collections.Generic.Dictionary<string, JToken>();
}
