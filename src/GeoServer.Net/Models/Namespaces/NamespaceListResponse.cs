using System.Collections.Generic;
using GeoServer.Models.Common;
using Newtonsoft.Json;

namespace GeoServer.Models.Namespaces;

/// <summary>
/// List response for namespaces endpoint.
/// </summary>
public sealed class NamespaceListResponse
{
    /// <summary>
    /// Envelope from GeoServer.
    /// </summary>
    [JsonProperty("namespaces")]
    public NamespaceListEnvelope Namespaces { get; set; } = new();
}

/// <summary>
/// Namespace list envelope.
/// </summary>
public sealed class NamespaceListEnvelope
{
    /// <summary>
    /// Namespace entries.
    /// </summary>
    [JsonProperty("namespace")]
    public List<NamedResourceDto> Namespace { get; set; } = new();
}
