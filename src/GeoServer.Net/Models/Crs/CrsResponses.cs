using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GeoServer.Models.Crs;

/// <summary>
/// CRS list response.
/// </summary>
public sealed class CrsListResponse
{
    /// <summary>
    /// CRS entries returned by the server.
    /// </summary>
    [JsonProperty("crs")]
    public List<CrsLinkDto> Crs { get; set; } = new();

    /// <summary>
    /// Pagination information.
    /// </summary>
    [JsonProperty("page")]
    public CrsPageInfo Page { get; set; } = new();
}

/// <summary>
/// CRS authorities response.
/// </summary>
public sealed class CrsAuthoritiesResponse
{
    /// <summary>
    /// Authorities collection.
    /// </summary>
    [JsonProperty("authorities")]
    public List<CrsAuthorityDto> Authorities { get; set; } = new();

    /// <summary>
    /// Backward-compatible alias for payloads using <c>authority</c>.
    /// </summary>
    [JsonProperty("authority")]
    private List<CrsAuthorityDto>? AuthorityAlias
    {
        set
        {
            if (value is not null && value.Count > 0)
            {
                Authorities = value;
            }
        }
    }
}

/// <summary>
/// CRS definition response.
/// </summary>
public sealed class CrsDefinitionResponse
{
    /// <summary>
    /// CRS identifier.
    /// </summary>
    [JsonProperty("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Definition format.
    /// </summary>
    [JsonProperty("format")]
    public string? Format { get; set; }

    /// <summary>
    /// CRS name.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// CRS validity bounding box.
    /// </summary>
    [JsonProperty("bbox")]
    public CrsBoundingBox? BBox { get; set; }

    /// <summary>
    /// CRS validity bounding box in WGS84.
    /// </summary>
    [JsonProperty("bboxWGS84")]
    public CrsBoundingBox? BBoxWgs84 { get; set; }

    /// <summary>
    /// WKT definition text.
    /// </summary>
    [JsonProperty("definition")]
    public string? Definition { get; set; }

    /// <summary>
    /// Additional unknown properties.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, JToken> AdditionalData { get; set; } = new Dictionary<string, JToken>();
}

/// <summary>
/// CRS link entry.
/// </summary>
public sealed class CrsLinkDto
{
    /// <summary>
    /// Identifier in AUTHORITY:CODE form.
    /// </summary>
    [JsonProperty("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Link to CRS resource.
    /// </summary>
    [JsonProperty("href")]
    public string? Href { get; set; }
}

/// <summary>
/// CRS authority entry.
/// </summary>
public sealed class CrsAuthorityDto
{
    /// <summary>
    /// Authority name.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Link to filtered CRS list for the authority.
    /// </summary>
    [JsonProperty("href")]
    public string? Href { get; set; }
}

/// <summary>
/// CRS pagination information.
/// </summary>
public sealed class CrsPageInfo
{
    /// <summary>
    /// Requested page offset.
    /// </summary>
    [JsonProperty("offset")]
    public int? Offset { get; set; }

    /// <summary>
    /// Requested page limit.
    /// </summary>
    [JsonProperty("limit")]
    public int? Limit { get; set; }

    /// <summary>
    /// Number of returned results.
    /// </summary>
    [JsonProperty("returned")]
    public int? Returned { get; set; }

    /// <summary>
    /// Total number of results.
    /// </summary>
    [JsonProperty("total")]
    public int? Total { get; set; }
}

/// <summary>
/// Bounding box used in CRS definition payloads.
/// </summary>
public sealed class CrsBoundingBox
{
    /// <summary>
    /// Minimum X value.
    /// </summary>
    [JsonProperty("minX")]
    public double? MinX { get; set; }

    /// <summary>
    /// Minimum Y value.
    /// </summary>
    [JsonProperty("minY")]
    public double? MinY { get; set; }

    /// <summary>
    /// Maximum X value.
    /// </summary>
    [JsonProperty("maxX")]
    public double? MaxX { get; set; }

    /// <summary>
    /// Maximum Y value.
    /// </summary>
    [JsonProperty("maxY")]
    public double? MaxY { get; set; }
}
