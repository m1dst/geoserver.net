using Newtonsoft.Json;

namespace GeoServer.Models.Styles;

/// <summary>
/// Single style response wrapper.
/// </summary>
public sealed class StyleResponse
{
    /// <summary>
    /// Style object.
    /// </summary>
    [JsonProperty("style")]
    public StyleDto Style { get; set; } = new();
}
