using Newtonsoft.Json;

namespace geoserver.net.Models.FeatureTypes;

/// <summary>
/// Request payload used to create or update a feature type.
/// </summary>
public sealed class FeatureTypeCreateRequest
{
    /// <summary>
    /// Feature type name.
    /// </summary>
    [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
    public string? Name { get; set; }

    /// <summary>
    /// Enabled flag.
    /// </summary>
    [JsonProperty("enabled", NullValueHandling = NullValueHandling.Ignore)]
    public bool? Enabled { get; set; }

    /// <summary>
    /// Native source name.
    /// </summary>
    [JsonProperty("nativeName", NullValueHandling = NullValueHandling.Ignore)]
    public string? NativeName { get; set; }

    /// <summary>
    /// Optional title.
    /// </summary>
    [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
    public string? Title { get; set; }
}
