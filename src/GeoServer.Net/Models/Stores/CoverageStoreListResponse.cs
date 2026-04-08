using System.Collections.Generic;
using Newtonsoft.Json;
using geoserver.net.Models.Common;

namespace geoserver.net.Models.Stores;

/// <summary>
/// List response for coverage stores.
/// </summary>
public sealed class CoverageStoreListResponse
{
    /// <summary>
    /// Envelope from GeoServer.
    /// </summary>
    [JsonProperty("coverageStores")]
    public CoverageStoreListEnvelope CoverageStores { get; set; } = new();
}

/// <summary>
/// Coverage store list envelope.
/// </summary>
public sealed class CoverageStoreListEnvelope
{
    /// <summary>
    /// Coverage store entries.
    /// </summary>
    [JsonProperty("coverageStore")]
    public List<NamedResourceDto> CoverageStore { get; set; } = new();
}
