using Newtonsoft.Json.Linq;

namespace geoserver.net.Models.Importer;

/// <summary>
/// Generic importer response wrapper.
/// </summary>
public sealed class ImportsResponse
{
    /// <summary>
    /// Raw importer payload.
    /// </summary>
    [Newtonsoft.Json.JsonExtensionData]
    public System.Collections.Generic.IDictionary<string, JToken> Payload { get; set; } = new System.Collections.Generic.Dictionary<string, JToken>();
}
