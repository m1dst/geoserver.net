using Newtonsoft.Json;

namespace GeoServer.Models.FeatureTypes;

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
