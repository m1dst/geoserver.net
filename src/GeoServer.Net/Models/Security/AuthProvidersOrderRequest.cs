using System.Collections.Generic;
using Newtonsoft.Json;

namespace geoserver.net.Models.Security;

/// <summary>
/// Authentication providers order request.
/// </summary>
public sealed class AuthProvidersOrderRequest
{
    /// <summary>
    /// Ordered provider names.
    /// </summary>
    [JsonProperty("order")]
    public List<string> Order { get; set; } = new();
}
