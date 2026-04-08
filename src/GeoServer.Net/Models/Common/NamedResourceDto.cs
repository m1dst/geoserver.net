using Newtonsoft.Json;

namespace geoserver.net.Models.Common;

/// <summary>
/// Common named GeoServer resource representation.
/// </summary>
public sealed class NamedResourceDto
{
    /// <summary>
    /// Resource name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Resource URI.
    /// </summary>
    [JsonProperty("href")]
    public string? Href { get; set; }
}
