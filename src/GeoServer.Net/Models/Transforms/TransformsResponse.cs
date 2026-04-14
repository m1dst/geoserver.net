using Newtonsoft.Json.Linq;

namespace GeoServer.Models.Transforms;

/// <summary>
/// Generic transforms response wrapper.
/// </summary>
public sealed class TransformsResponse
{
    /// <summary>
    /// Raw transforms payload.
    /// </summary>
    [Newtonsoft.Json.JsonExtensionData]
    public System.Collections.Generic.IDictionary<string, JToken> Payload { get; set; } = new System.Collections.Generic.Dictionary<string, JToken>();
}
