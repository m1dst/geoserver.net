using System.Collections.Generic;
using geoserver.net.Models.Common;
using Newtonsoft.Json;

namespace geoserver.net.Models.Coverages;

/// <summary>
/// List response for coverages endpoint.
/// </summary>
public sealed class CoverageListResponse
{
    /// <summary>
    /// Envelope from GeoServer.
    /// </summary>
    [JsonProperty("coverages")]
    public CoverageListEnvelope Coverages { get; set; } = new();
}

/// <summary>
/// Coverage list envelope.
/// </summary>
public sealed class CoverageListEnvelope
{
    /// <summary>
    /// Coverage entries.
    /// </summary>
    [JsonProperty("coverage")]
    public List<NamedResourceDto> Coverage { get; set; } = new();
}
