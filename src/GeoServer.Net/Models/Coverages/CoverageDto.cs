using Newtonsoft.Json;

namespace GeoServer.Models.Coverages;

/// <summary>
/// Coverage representation.
/// </summary>
public sealed class CoverageDto
{
    /// <summary>
    /// Coverage name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Optional title.
    /// </summary>
    [JsonProperty("title")]
    public string? Title { get; set; }
}
