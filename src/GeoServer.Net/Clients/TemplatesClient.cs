using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GeoServer.Models.Templates;

namespace GeoServer.Clients;

/// <summary>
/// Wrapper around templates endpoints.
/// </summary>
public sealed class TemplatesClient : GeoServerClientBase
{
    internal TemplatesClient(HttpClient httpClient, GeoServerRequestContext? requestContext = null) : base(httpClient, requestContext)
    {
    }

    /// <summary>
    /// Lists templates for the specified scope path.
    /// </summary>
    public Task<TemplatesResponse> GetAllAsync(string scopePath = "templates", CancellationToken cancellationToken = default)
        => SendAsync<TemplatesResponse>(HttpMethod.Get, NormalizeScopePath(scopePath), cancellationToken: cancellationToken);

    /// <summary>
    /// Lists templates for the specified scope path (synchronous).
    /// </summary>
    public TemplatesResponse GetAll(string scopePath = "templates")
        => Send<TemplatesResponse>(HttpMethod.Get, NormalizeScopePath(scopePath));

    /// <summary>
    /// Gets a template's text content by scope path and template name.
    /// </summary>
    public Task<string> GetTemplateRawAsync(string scopePath, string templateName, CancellationToken cancellationToken = default)
        => SendRawAsync(HttpMethod.Get, BuildTemplatePath(scopePath, templateName), cancellationToken: cancellationToken);

    /// <summary>
    /// Gets a template's text content by scope path and template name (synchronous).
    /// </summary>
    public string GetTemplateRaw(string scopePath, string templateName)
        => SendRaw(HttpMethod.Get, BuildTemplatePath(scopePath, templateName));

    /// <summary>
    /// Inserts or updates a template.
    /// </summary>
    public async Task PutTemplateAsync(string scopePath, string templateName, string content, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, BuildTemplatePath(scopePath, templateName))
        {
            Content = new StringContent(content ?? string.Empty, Encoding.UTF8, "text/plain")
        };

        using var response = await HttpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var body = response.Content is null ? string.Empty : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new GeoServerApiException(response.StatusCode, body, $"GeoServer REST call failed for '{request.RequestUri}'.");
        }
    }

    /// <summary>
    /// Inserts or updates a template (synchronous).
    /// </summary>
    public void PutTemplate(string scopePath, string templateName, string content)
        => PutTemplateAsync(scopePath, templateName, content).GetAwaiter().GetResult();

    /// <summary>
    /// Deletes a template.
    /// </summary>
    public Task DeleteTemplateAsync(string scopePath, string templateName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, BuildTemplatePath(scopePath, templateName), cancellationToken: cancellationToken);

    /// <summary>
    /// Deletes a template (synchronous).
    /// </summary>
    public void DeleteTemplate(string scopePath, string templateName)
        => Send(HttpMethod.Delete, BuildTemplatePath(scopePath, templateName));

    private static string BuildTemplatePath(string scopePath, string templateName)
    {
        var path = NormalizeScopePath(scopePath);
        var name = EncodeRequired(templateName, nameof(templateName));
        return $"{path}/{name}.ftl";
    }

    private static string NormalizeScopePath(string scopePath)
    {
        if (string.IsNullOrWhiteSpace(scopePath))
        {
            throw new ArgumentException("Value is required.", nameof(scopePath));
        }

        var segments = scopePath.Trim('/').Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length == 0)
        {
            throw new ArgumentException("Value is required.", nameof(scopePath));
        }

        for (var i = 0; i < segments.Length; i++)
        {
            segments[i] = EncodeRequired(segments[i], nameof(scopePath));
        }

        return string.Join("/", segments);
    }

    private static string EncodeRequired(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is required.", paramName);
        }

        return Uri.EscapeDataString(value);
    }
}
