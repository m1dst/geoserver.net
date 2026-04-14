using System.Collections.Generic;
using Newtonsoft.Json;

namespace GeoServer.Models.Fonts;

/// <summary>
/// Fonts response wrapper.
/// </summary>
public sealed class FontsResponse
{
    /// <summary>
    /// Installed font names.
    /// </summary>
    [JsonProperty("fonts")]
    public List<string> Fonts { get; set; } = new();
}
