using System.Collections.Generic;
using geoserver.net.Models.Common;
using Newtonsoft.Json;

namespace geoserver.net.Models.LayerGroups;

/// <summary>
/// List response for layer groups endpoint.
/// </summary>
public sealed class LayerGroupListResponse
{
    /// <summary>
    /// Envelope from GeoServer.
    /// </summary>
    [JsonProperty("layerGroups")]
    public LayerGroupListEnvelope LayerGroups { get; set; } = new();
}

/// <summary>
/// Layer group list envelope.
/// </summary>
public sealed class LayerGroupListEnvelope
{
    /// <summary>
    /// Layer group entries.
    /// </summary>
    [JsonProperty("layerGroup")]
    public List<NamedResourceDto> LayerGroup { get; set; } = new();
}
