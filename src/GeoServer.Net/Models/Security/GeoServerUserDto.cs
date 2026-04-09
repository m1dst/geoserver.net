using Newtonsoft.Json;

namespace geoserver.net.Models.Security;

/// <summary>
/// GeoServer security user DTO.
/// </summary>
public sealed class GeoServerUserDto
{
    /// <summary>
    /// Username.
    /// </summary>
    [JsonProperty("userName")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Password.
    /// </summary>
    [JsonProperty("password")]
    public string? Password { get; set; }

    /// <summary>
    /// Enabled flag.
    /// </summary>
    [JsonProperty("enabled")]
    public bool Enabled { get; set; }
}
