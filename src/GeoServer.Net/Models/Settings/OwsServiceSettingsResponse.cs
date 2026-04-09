using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace geoserver.net.Models.Settings;

/// <summary>
/// OWS service settings response with dynamic root key (wms/wfs/wcs/wmts).
/// </summary>
public sealed class OwsServiceSettingsResponse
{
    /// <summary>
    /// Raw root payload.
    /// </summary>
    [JsonExtensionData]
    public System.Collections.Generic.IDictionary<string, JToken> Payload { get; set; } = new System.Collections.Generic.Dictionary<string, JToken>();
}
