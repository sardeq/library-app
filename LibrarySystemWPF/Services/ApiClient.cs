using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using LibrarySystemWPF.Models;

namespace LibrarySystemWPF.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:44381/api/";
        private static readonly string LogFilePath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "requestLog.txt");



        public ApiClient()
        {
            _httpClient = new HttpClient();
            if (!string.IsNullOrEmpty(UserSession.Token))
            {
                SetToken(UserSession.Token);
            }
        }

        public void SetToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        private async Task LogRequest(string endpoint, string method, object body = null)
        {
            using (StreamWriter sw = new StreamWriter(LogFilePath, append: true))
            {
                await sw.WriteLineAsync("=== API REQUEST ===");
                await sw.WriteLineAsync($"Time: {DateTime.Now}");
                await sw.WriteLineAsync($"Endpoint: {endpoint}");
                await sw.WriteLineAsync($"Method: {method}");
                if (_httpClient.DefaultRequestHeaders.Authorization != null)
                    await sw.WriteLineAsync($"Authorization: {_httpClient.DefaultRequestHeaders.Authorization}");
                if (body != null)
                    await sw.WriteLineAsync($"Body: {JsonSerializer.Serialize(body)}");
                await sw.WriteLineAsync("===================\n");
            }
        }

        public async Task<LoginResult> Login(string username, string password)
        {
            var endpoint = $"{BaseUrl}auth/login";
            var body = new { Username = username, Password = password };
            await LogRequest(endpoint, "POST", body);

            var response = await _httpClient.PostAsJsonAsync(endpoint, body);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LoginResult>();
            }
            else
            {
                return null;
            }
        }

        public async Task<List<User>> GetAllUsers()
        {
            var endpoint = $"{BaseUrl}users";
            await LogRequest(endpoint, "GET");

            return await _httpClient.GetFromJsonAsync<List<User>>(endpoint);
        }

        public async Task<bool> SaveUser(User user)
        {
            var endpoint = $"{BaseUrl}users";
            await LogRequest(endpoint, "POST", user);

            var response = await _httpClient.PostAsJsonAsync(endpoint, user);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var endpoint = $"{BaseUrl}users/{id}";
            await LogRequest(endpoint, "DELETE");

            var response = await _httpClient.DeleteAsync(endpoint);
            return response.IsSuccessStatusCode;
        }
    }
}