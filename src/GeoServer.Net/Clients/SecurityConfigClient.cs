using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using geoserver.net.Models.Security;

namespace geoserver.net.Clients;

/// <summary>
/// Wrapper around root GeoServer security configuration endpoints.
/// </summary>
public sealed class SecurityConfigClient : GeoServerClientBase
{
    internal SecurityConfigClient(HttpClient httpClient) : base(httpClient)
    {
    }

    /// <summary>
    /// Gets the current master password metadata.
    /// </summary>
    public Task<MasterPasswordResponse> GetMasterPasswordAsync(CancellationToken cancellationToken = default)
        => SendAsync<MasterPasswordResponse>(HttpMethod.Get, "security/masterpw", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets the current master password metadata (synchronous).
    /// </summary>
    public MasterPasswordResponse GetMasterPassword()
        => Send<MasterPasswordResponse>(HttpMethod.Get, "security/masterpw");

    /// <summary>
    /// Updates the master password.
    /// </summary>
    public Task UpdateMasterPasswordAsync(UpdateMasterPasswordRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, "security/masterpw", Validate(request), cancellationToken);

    /// <summary>
    /// Updates the master password (synchronous).
    /// </summary>
    public void UpdateMasterPassword(UpdateMasterPasswordRequest request)
        => Send(HttpMethod.Put, "security/masterpw", Validate(request));

    /// <summary>
    /// Updates the password of the currently authenticated user.
    /// </summary>
    public Task UpdateSelfPasswordAsync(SelfPasswordRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, "security/self/password", Validate(request), cancellationToken);

    /// <summary>
    /// Updates the password of the currently authenticated user (synchronous).
    /// </summary>
    public void UpdateSelfPassword(SelfPasswordRequest request)
        => Send(HttpMethod.Put, "security/self/password", Validate(request));

    /// <summary>
    /// Gets catalog security mode.
    /// </summary>
    public Task<CatalogModeResponse> GetCatalogModeAsync(CancellationToken cancellationToken = default)
        => SendAsync<CatalogModeResponse>(HttpMethod.Get, "security/acl/catalog", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets catalog security mode (synchronous).
    /// </summary>
    public CatalogModeResponse GetCatalogMode()
        => Send<CatalogModeResponse>(HttpMethod.Get, "security/acl/catalog");

    /// <summary>
    /// Updates catalog security mode.
    /// </summary>
    public Task UpdateCatalogModeAsync(CatalogModeRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, "security/acl/catalog", Validate(request), cancellationToken);

    /// <summary>
    /// Updates catalog security mode (synchronous).
    /// </summary>
    public void UpdateCatalogMode(CatalogModeRequest request)
        => Send(HttpMethod.Put, "security/acl/catalog", Validate(request));

    /// <summary>
    /// Reloads security ACL catalog and configuration.
    /// </summary>
    public Task ReloadAclCatalogAsync(bool usePost = false, CancellationToken cancellationToken = default)
        => SendAsync(usePost ? HttpMethod.Post : HttpMethod.Put, "security/acl/catalog/reload", cancellationToken: cancellationToken);

    /// <summary>
    /// Reloads security ACL catalog and configuration (synchronous).
    /// </summary>
    public void ReloadAclCatalog(bool usePost = false)
        => Send(usePost ? HttpMethod.Post : HttpMethod.Put, "security/acl/catalog/reload");

    private static UpdateMasterPasswordRequest Validate(UpdateMasterPasswordRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.OldMasterPassword))
        {
            throw new ArgumentException("Old master password is required.", nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.NewMasterPassword))
        {
            throw new ArgumentException("New master password is required.", nameof(request));
        }

        return request;
    }

    private static SelfPasswordRequest Validate(SelfPasswordRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.NewPassword))
        {
            throw new ArgumentException("New password is required.", nameof(request));
        }

        return request;
    }

    private static CatalogModeRequest Validate(CatalogModeRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.Mode))
        {
            throw new ArgumentException("Catalog mode is required.", nameof(request));
        }

        return request;
    }
}
