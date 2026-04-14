using Newtonsoft.Json.Linq;

namespace GeoServer.Models.Security;

/// <summary>
/// Authentication filters response wrapper.
/// </summary>
public sealed class AuthFiltersResponse
{
    /// <summary>
    /// Raw payload.
    /// </summary>
    [Newtonsoft.Json.JsonExtensionData]
    public System.Collections.Generic.IDictionary<string, JToken> Payload { get; set; } = new System.Collections.Generic.Dictionary<string, JToken>();
}
