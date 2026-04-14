using Newtonsoft.Json;

namespace GeoServer.Models.Coverages;

/// <summary>
/// Request payload used to create or update a coverage.
/// </summary>
public sealed class CoverageCreateRequest
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

    /// <summary>
    /// Optional native source name.
    /// </summary>
    [JsonProperty("nativeName")]
    public string? NativeName { get; set; }
}
