using Newtonsoft.Json;

namespace geoserver.net.Models.Security;

/// <summary>
/// Response payload for the current master password endpoint.
/// </summary>
public sealed class MasterPasswordResponse
{
    /// <summary>
    /// Current master password value.
    /// </summary>
    [JsonProperty("oldMasterPassword")]
    public string? OldMasterPassword { get; set; }
}

/// <summary>
/// Request payload for updating the master password.
/// </summary>
public sealed class UpdateMasterPasswordRequest
{
    /// <summary>
    /// Existing master password.
    /// </summary>
    [JsonProperty("oldMasterPassword")]
    public string OldMasterPassword { get; set; } = string.Empty;

    /// <summary>
    /// New master password.
    /// </summary>
    [JsonProperty("newMasterPassword")]
    public string NewMasterPassword { get; set; } = string.Empty;
}

/// <summary>
/// Request payload for updating the current user password.
/// </summary>
public sealed class SelfPasswordRequest
{
    /// <summary>
    /// New password value.
    /// </summary>
    [JsonProperty("newPassword")]
    public string NewPassword { get; set; } = string.Empty;
}

/// <summary>
/// Catalog mode response payload.
/// </summary>
public sealed class CatalogModeResponse
{
    /// <summary>
    /// Catalog mode (HIDE, MIXED, CHALLENGE).
    /// </summary>
    [JsonProperty("mode")]
    public string? Mode { get; set; }

    /// <summary>
    /// Backward-compatible alias for nested payloads.
    /// </summary>
    [JsonProperty("catalog")]
    private CatalogModeResponse? CatalogAlias
    {
        set
        {
            if (!string.IsNullOrWhiteSpace(value?.Mode))
            {
                Mode = value.Mode;
            }
        }
    }
}

/// <summary>
/// Catalog mode update request payload.
/// </summary>
public sealed class CatalogModeRequest
{
    /// <summary>
    /// Catalog mode (HIDE, MIXED, CHALLENGE).
    /// </summary>
    [JsonProperty("mode")]
    public string Mode { get; set; } = string.Empty;
}
