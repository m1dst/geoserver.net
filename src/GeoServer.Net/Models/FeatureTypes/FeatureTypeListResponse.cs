using System.Collections.Generic;
using Newtonsoft.Json;
using geoserver.net.Models.Common;

namespace geoserver.net.Models.FeatureTypes;

/// <summary>
/// List response for feature types.
/// </summary>
public sealed class FeatureTypeListResponse
{
    /// <summary>
    /// Envelope from GeoServer.
    /// </summary>
    [JsonProperty("featureTypes")]
    public FeatureTypeListEnvelope FeatureTypes { get; set; } = new();
}

/// <summary>
/// Feature type list envelope.
/// </summary>
public sealed class FeatureTypeListEnvelope
{
    /// <summary>
    /// Feature type entries.
    /// </summary>
    [JsonProperty("featureType")]
    public List<NamedResourceDto> FeatureType { get; set; } = new();
}
