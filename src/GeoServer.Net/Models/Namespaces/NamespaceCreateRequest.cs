using Newtonsoft.Json;

namespace geoserver.net.Models.Namespaces;

/// <summary>
/// Request payload used to create a namespace.
/// </summary>
public sealed class NamespaceCreateRequest
{
    /// <summary>
    /// Namespace prefix.
    /// </summary>
    [JsonProperty("prefix")]
    public string Prefix { get; set; } = string.Empty;

    /// <summary>
    /// Namespace URI.
    /// </summary>
    [JsonProperty("uri")]
    public string Uri { get; set; } = string.Empty;
}
