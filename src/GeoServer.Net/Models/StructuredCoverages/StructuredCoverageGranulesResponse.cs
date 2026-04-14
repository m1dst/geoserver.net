using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GeoServer.Models.StructuredCoverages;

/// <summary>
/// Structured coverage granules collection response.
/// </summary>
public sealed class StructuredCoverageGranulesResponse
{
    /// <summary>
    /// Raw JSON properties (typically a GeoJSON FeatureCollection document).
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, JToken> Payload { get; set; } = new Dictionary<string, JToken>();
}
