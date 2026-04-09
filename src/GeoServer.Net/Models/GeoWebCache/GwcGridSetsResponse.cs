using Newtonsoft.Json.Linq;

namespace geoserver.net.Models.GeoWebCache;

/// <summary>
/// GeoWebCache grid sets response wrapper.
/// </summary>
public sealed class GwcGridSetsResponse
{
    /// <summary>
    /// Raw grid sets payload.
    /// </summary>
    [Newtonsoft.Json.JsonExtensionData]
    public System.Collections.Generic.IDictionary<string, JToken> Payload { get; set; } = new System.Collections.Generic.Dictionary<string, JToken>();
}
