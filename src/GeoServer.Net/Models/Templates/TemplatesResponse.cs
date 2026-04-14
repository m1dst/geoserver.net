using Newtonsoft.Json.Linq;

namespace GeoServer.Models.Templates;

/// <summary>
/// Generic templates list response wrapper.
/// </summary>
public sealed class TemplatesResponse
{
    /// <summary>
    /// Raw templates payload.
    /// </summary>
    [Newtonsoft.Json.JsonExtensionData]
    public System.Collections.Generic.IDictionary<string, JToken> Payload { get; set; } = new System.Collections.Generic.Dictionary<string, JToken>();
}
