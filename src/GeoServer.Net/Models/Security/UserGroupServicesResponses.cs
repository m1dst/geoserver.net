using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace geoserver.net.Models.Security;

/// <summary>
/// User/group services list response.
/// </summary>
public sealed class UserGroupServicesListResponse
{
    /// <summary>
    /// Service summaries.
    /// </summary>
    public List<UserGroupServiceSummaryDto> UserGroupServices { get; set; } = new();
}

/// <summary>
/// User/group service summary item.
/// </summary>
public sealed class UserGroupServiceSummaryDto
{
    /// <summary>
    /// Service name.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Concrete service class name.
    /// </summary>
    [JsonProperty("className")]
    public string? ClassName { get; set; }
}

/// <summary>
/// User/group service configuration response wrapper.
/// </summary>
public sealed class UserGroupServiceConfigResponse
{
    /// <summary>
    /// Raw response payload.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, JToken> Payload { get; set; } = new Dictionary<string, JToken>();
}
