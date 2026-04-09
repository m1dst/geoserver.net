using System.Collections.Generic;
using Newtonsoft.Json;

namespace geoserver.net.Models.Security;

/// <summary>
/// Groups list response.
/// </summary>
public sealed class GroupsListResponse
{
    /// <summary>
    /// Groups.
    /// </summary>
    [JsonProperty("groups")]
    public List<string> Groups { get; set; } = new();
}
