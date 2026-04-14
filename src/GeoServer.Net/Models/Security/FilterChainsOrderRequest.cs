using System.Collections.Generic;
using Newtonsoft.Json;

namespace GeoServer.Models.Security;

/// <summary>
/// Filter chain ordering request payload.
/// </summary>
public sealed class FilterChainsOrderRequest
{
    /// <summary>
    /// Ordered filter chain names.
    /// </summary>
    [JsonProperty("order")]
    public List<string> Order { get; set; } = new();
}
