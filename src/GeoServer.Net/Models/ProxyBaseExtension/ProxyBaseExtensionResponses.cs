using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace geoserver.net.Models.ProxyBaseExtension;

/// <summary>
/// Response wrapper for proxy base extension rule collections.
/// </summary>
public sealed class ProxyBaseExtensionRuleListResponse
{
    /// <summary>
    /// Raw response payload.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, JToken> Payload { get; set; } = new Dictionary<string, JToken>();
}

/// <summary>
/// Response wrapper for a single proxy base extension rule.
/// </summary>
public sealed class ProxyBaseExtensionRuleResponse
{
    /// <summary>
    /// Raw response payload.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, JToken> Payload { get; set; } = new Dictionary<string, JToken>();
}
