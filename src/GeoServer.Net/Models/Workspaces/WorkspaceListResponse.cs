using System.Collections.Generic;
using Newtonsoft.Json;
using geoserver.net.Models.Common;

namespace geoserver.net.Models.Workspaces;

/// <summary>
/// List response for workspaces endpoint.
/// </summary>
public sealed class WorkspaceListResponse
{
    /// <summary>
    /// Envelope from GeoServer.
    /// </summary>
    [JsonProperty("workspaces")]
    public WorkspaceListEnvelope Workspaces { get; set; } = new();
}

/// <summary>
/// Workspace list envelope.
/// </summary>
public sealed class WorkspaceListEnvelope
{
    /// <summary>
    /// Workspace items.
    /// </summary>
    [JsonProperty("workspace")]
    public List<NamedResourceDto> Workspace { get; set; } = new();
}
