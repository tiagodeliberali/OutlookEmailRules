namespace OutlookEmailRules.Core;

using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Graph.Models;

public class GraphApiService : IGraphApiService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _options;

    public GraphApiService(string accessToken) : this(new AuthenticationHeaderValue("Bearer", accessToken))
    {
    }

    public GraphApiService(AuthenticationHeaderValue authHeader)
    {
        _httpClient = new HttpClient();

        // Add the access token to the Authorization header of the HttpRequestMessage object
        _httpClient.DefaultRequestHeaders.Authorization = authHeader;

        _options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task<List<MailFolder>> GetFoldersAsync()
    {
        // Define Graph API endpoint to get all the mail folders for the user
        string graphApiEndpoint = "https://graph.microsoft.com/v1.0/me/mailFolders?$expand=childFolders";

        // Call Graph API endpoint and return the list of mailFolders
        return await CallGraphApiEndpoint<MailFolder>(graphApiEndpoint);
    }

    // get a list of folder children of a given folderid
    public async Task<List<MailFolder>> GetFolderChildrenAsync(string folderId)
    {
        // Define Graph API endpoint to get all the mail folders for the user
        string graphApiEndpoint = $"https://graph.microsoft.com/v1.0/me/mailFolders/{folderId}/childFolders";

        // Call Graph API endpoint and return the list of mailFolders
        return await CallGraphApiEndpoint<MailFolder>(graphApiEndpoint);
    }

    public async Task<List<MessageRule>> GetRulesAsync()
    {
        // Define your application details
        string graphApiEndpoint = "https://graph.microsoft.com/v1.0/me/mailFolders/inbox/messageRules";

        // call Graph API endpoint and return the JSON string
        return await CallGraphApiEndpoint<MessageRule>(graphApiEndpoint);
    }

    private async Task<List<T>> CallGraphApiEndpoint<T>(string endpoint)
    {
        // Send the GET request to the Graph API endpoint
        var response = await _httpClient.GetAsync(endpoint);

        var content = await response.Content.ReadAsStringAsync();

        // If the response is not successful, throw an exception
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error calling Graph API. StatusCode=${response.StatusCode}\n\nError: {content}");
        }

        var list = JsonSerializer.Deserialize<ApiResponse<T>>(content, _options);
        return list?.Value ?? new(0);
    }
}
