using System.Collections.Generic;
using Newtonsoft.Json;

namespace GeoServer.Models.Security;

/// <summary>
/// Users list response.
/// </summary>
public sealed class UsersListResponse
{
    /// <summary>
    /// Users.
    /// </summary>
    [JsonProperty("users")]
    public List<GeoServerUserDto> Users { get; set; } = new();
}
