﻿using UI.Dto;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Net.Http.Json;

namespace UI;
public class ApiClient
{
    private readonly HttpClient httpClient;

    public ApiClient()
    {
        httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5201")
        };
        var token = TokenFileStorage.GetToken();
        if (token != null)
        {
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }

    private async Task<string> PostAsync<T>(string endpoint, object data)
    {
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(endpoint, content);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        return responseString;
        //return JsonSerializer.Deserialize<T>(responseJson);
    }

    public async Task<string> RegisterAsync(RegisterUserRequest request)
    {
        return await PostAsync<string>("/register", request);
    }

    public async Task<string> LoginAsync(LoginUserRequest request)
    {
        return await PostAsync<string>("/login", request);
    }
}
