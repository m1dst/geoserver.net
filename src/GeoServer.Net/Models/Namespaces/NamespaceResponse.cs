using Newtonsoft.Json;

namespace geoserver.net.Models.Namespaces;

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
