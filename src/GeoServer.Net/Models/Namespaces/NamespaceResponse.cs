using Newtonsoft.Json;

namespace GeoServer.Models.Namespaces;

/// <summary>
/// Single namespace response wrapper.
/// </summary>
public sealed class NamespaceResponse
{
    /// <summary>
    /// Namespace object.
    /// </summary>
    [JsonProperty("namespace")]
    public NamespaceDto Namespace { get; set; } = new();
}
