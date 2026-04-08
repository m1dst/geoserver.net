using Newtonsoft.Json;

namespace geoserver.net.Models.FeatureTypes;

/// <summary>
/// Single feature type response wrapper.
/// </summary>
public sealed class FeatureTypeResponse
{
    /// <summary>
    /// Feature type object.
    /// </summary>
    [JsonProperty("featureType")]
    public FeatureTypeDto FeatureType { get; set; } = new();
}
