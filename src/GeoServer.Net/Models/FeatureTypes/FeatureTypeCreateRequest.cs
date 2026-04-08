using Newtonsoft.Json;

namespace geoserver.net.Models.FeatureTypes;

/// <summary>
/// Request payload used to create a feature type.
/// </summary>
public sealed class FeatureTypeCreateRequest
{
    /// <summary>
    /// Feature type name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Native source name.
    /// </summary>
    [JsonProperty("nativeName")]
    public string? NativeName { get; set; }

    /// <summary>
    /// Optional title.
    /// </summary>
    [JsonProperty("title")]
    public string? Title { get; set; }
}
