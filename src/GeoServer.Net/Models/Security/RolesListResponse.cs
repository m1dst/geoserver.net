using System.Collections.Generic;
using Newtonsoft.Json;

namespace GeoServer.Models.Security;

/// <summary>
/// Roles list response.
/// </summary>
public sealed class RolesListResponse
{
    /// <summary>
    /// Role names.
    /// </summary>
    [JsonProperty("roles")]
    public List<string> Roles { get; set; } = new();
}
