using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using GeoServer.Models.Security;
using Newtonsoft.Json.Linq;

namespace GeoServer.Clients;

/// <summary>
/// Wrapper around GeoServer user/group service configuration endpoints.
/// </summary>
public sealed class UserGroupServicesClient : GeoServerClientBase
{
    internal UserGroupServicesClient(HttpClient httpClient, GeoServerRequestContext? requestContext = null) : base(httpClient, requestContext)
    {
    }

    /// <summary>
    /// Lists configured user/group services.
    /// </summary>
    public async Task<UserGroupServicesListResponse> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var token = await SendRawAsTokenAsync("security/usergroupservices", cancellationToken).ConfigureAwait(false);
        return ParseList(token);
    }

    /// <summary>
    /// Lists configured user/group services (synchronous).
    /// </summary>
    public UserGroupServicesListResponse GetAll()
        => GetAllAsync().GetAwaiter().GetResult();

    /// <summary>
    /// Gets a user/group service configuration by name.
    /// </summary>
    public async Task<UserGroupServiceConfigResponse> GetByNameAsync(string serviceName, CancellationToken cancellationToken = default)
    {
        var raw = await SendRawAsync(HttpMethod.Get, $"security/usergroupservices/{Encode(serviceName)}", cancellationToken: cancellationToken).ConfigureAwait(false);
        return ParseConfig(raw);
    }

    /// <summary>
    /// Gets a user/group service configuration by name (synchronous).
    /// </summary>
    public UserGroupServiceConfigResponse GetByName(string serviceName)
        => GetByNameAsync(serviceName).GetAwaiter().GetResult();

    /// <summary>
    /// Creates a user/group service configuration.
    /// </summary>
    public Task CreateAsync(object serviceConfigPayload, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, "security/usergroupservices", serviceConfigPayload, cancellationToken);

    /// <summary>
    /// Creates a user/group service configuration (synchronous).
    /// </summary>
    public void Create(object serviceConfigPayload)
        => Send(HttpMethod.Post, "security/usergroupservices", serviceConfigPayload);

    /// <summary>
    /// Updates a user/group service configuration.
    /// </summary>
    public Task UpdateAsync(string serviceName, object serviceConfigPayload, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, $"security/usergroupservices/{Encode(serviceName)}", serviceConfigPayload, cancellationToken);

    /// <summary>
    /// Updates a user/group service configuration (synchronous).
    /// </summary>
    public void Update(string serviceName, object serviceConfigPayload)
        => Send(HttpMethod.Put, $"security/usergroupservices/{Encode(serviceName)}", serviceConfigPayload);

    /// <summary>
    /// Deletes a user/group service configuration.
    /// </summary>
    public Task DeleteAsync(string serviceName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"security/usergroupservices/{Encode(serviceName)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Deletes a user/group service configuration (synchronous).
    /// </summary>
    public void Delete(string serviceName)
        => Send(HttpMethod.Delete, $"security/usergroupservices/{Encode(serviceName)}");

    private async Task<JToken> SendRawAsTokenAsync(string path, CancellationToken cancellationToken)
    {
        var raw = await SendRawAsync(HttpMethod.Get, path, cancellationToken: cancellationToken).ConfigureAwait(false);
        return TryParseListToken(raw);
    }

    private static JToken TryParseListToken(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            return new JObject();
        }

        try
        {
            return JToken.Parse(raw);
        }
        catch (Newtonsoft.Json.JsonReaderException)
        {
            return ParseListXml(raw);
        }
    }

    private static UserGroupServicesListResponse ParseList(JToken token)
    {
        var result = new UserGroupServicesListResponse();

        if (token is JArray array)
        {
            AddArrayItems(array, result.UserGroupServices);
            return result;
        }

        if (token is JObject wrapped)
        {
            var candidate = wrapped["userGroupService"] ?? wrapped["usergroupservice"] ?? wrapped["services"];
            if (candidate is JArray candidateArray)
            {
                AddArrayItems(candidateArray, result.UserGroupServices);
                return result;
            }

            if (candidate is JObject single)
            {
                AddObjectItem(single, result.UserGroupServices);
                return result;
            }

            // Some payloads may include list at root-level properties.
            AddObjectItem(wrapped, result.UserGroupServices);
        }

        return result;
    }

    private static JObject ParseListXml(string raw)
    {
        var document = XDocument.Parse(raw);
        var root = document.Root;
        if (root is null)
        {
            return new JObject();
        }

        var summaries = new JArray();
        foreach (var element in root.Descendants())
        {
            if (!string.Equals(element.Name.LocalName, "userGroupService", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var name = element.Element(XName.Get("name"))?.Value;
            var className = element.Element(XName.Get("className"))?.Value ?? element.Element(XName.Get("class"))?.Value;
            if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(className))
            {
                continue;
            }

            var item = new JObject();
            if (!string.IsNullOrWhiteSpace(name))
            {
                item["name"] = name;
            }

            if (!string.IsNullOrWhiteSpace(className))
            {
                item["className"] = className;
            }

            summaries.Add(item);
        }

        if (summaries.Count == 0 && string.Equals(root.Name.LocalName, "userGroupService", StringComparison.OrdinalIgnoreCase))
        {
            var singleName = root.Element(XName.Get("name"))?.Value;
            var singleClass = root.Element(XName.Get("className"))?.Value ?? root.Element(XName.Get("class"))?.Value;
            if (!string.IsNullOrWhiteSpace(singleName) || !string.IsNullOrWhiteSpace(singleClass))
            {
                var single = new JObject();
                if (!string.IsNullOrWhiteSpace(singleName))
                {
                    single["name"] = singleName;
                }

                if (!string.IsNullOrWhiteSpace(singleClass))
                {
                    single["className"] = singleClass;
                }

                summaries.Add(single);
            }
        }

        return new JObject
        {
            ["userGroupService"] = summaries
        };
    }

    private static UserGroupServiceConfigResponse ParseConfig(string raw)
    {
        var response = new UserGroupServiceConfigResponse();
        if (string.IsNullOrWhiteSpace(raw))
        {
            return response;
        }

        try
        {
            var token = JToken.Parse(raw);
            if (token is JObject obj)
            {
                foreach (var property in obj.Properties())
                {
                    response.Payload[property.Name] = property.Value;
                }

                return response;
            }

            response.Payload["value"] = token;
            return response;
        }
        catch (Newtonsoft.Json.JsonReaderException)
        {
            response.Payload["rawXml"] = raw;
            return response;
        }
    }

    private static void AddArrayItems(JArray array, IList<UserGroupServiceSummaryDto> target)
    {
        for (var i = 0; i < array.Count; i++)
        {
            if (array[i] is JObject obj)
            {
                AddObjectItem(obj, target);
            }
        }
    }

    private static void AddObjectItem(JObject obj, IList<UserGroupServiceSummaryDto> target)
    {
        var name = (string?)obj["name"];
        var className = (string?)obj["className"] ?? (string?)obj["class"];
        if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(className))
        {
            return;
        }

        target.Add(new UserGroupServiceSummaryDto
        {
            Name = name,
            ClassName = className
        });
    }

    private static string Encode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is required.", nameof(value));
        }

        return Uri.EscapeDataString(value);
    }
}
