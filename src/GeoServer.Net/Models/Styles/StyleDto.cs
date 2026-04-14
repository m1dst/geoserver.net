using Newtonsoft.Json;

namespace GeoServer.Models.Styles;

/// <summary>
/// Style representation.
/// </summary>
public sealed class StyleDto
{
    /// <summary>
    /// Style name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Optional style format.
    /// </summary>
    [JsonProperty("format")]
    public string? Format { get; set; }
}
