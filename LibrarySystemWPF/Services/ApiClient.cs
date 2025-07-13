using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using LibrarySystemWPF.Models;

namespace LibrarySystemWPF.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:44381/api/";

        public ApiClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _httpClient.GetFromJsonAsync<List<User>>($"{BaseUrl}users");
        }

        public async Task<bool> SaveUser(User user)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}users", user);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}users/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}