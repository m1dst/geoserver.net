using Newtonsoft.Json;

namespace geoserver.net.Models.Styles;

/// <summary>
/// Request payload for creating a style metadata entry.
/// </summary>
public sealed class StyleCreateRequest
{
    /// <summary>
    /// Style name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Style filename.
    /// </summary>
    [JsonProperty("filename")]
    public string? Filename { get; set; }
}
