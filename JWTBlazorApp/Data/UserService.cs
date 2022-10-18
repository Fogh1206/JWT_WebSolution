using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JWTBlazorApp.Models;

namespace JWTBlazorApp.Data
{
    public class UserService : IUser
    {
        private const string Uri = "https://localhost:7166";
        private readonly HttpClient _client;

        public UserService()
        {
            _client = new HttpClient();
        }

        public async Task<AuthenticatedUserModel> AddUser(AuthenticationUserModel inputAuthenticationUser)
        {
            string userAsJson = JsonSerializer.Serialize(inputAuthenticationUser);
            HttpContent content = new StringContent(userAsJson,
                Encoding.UTF8,
                "application/json");
            
            HttpResponseMessage response = await _client.PostAsync(
                Uri + "/api/Auth/register", content);

            var result = await response.Content.ReadFromJsonAsync<AuthenticatedUserModel>();

            return result;
        }

        public async Task<string> LoginUser(AuthenticationUserModel inputAuthenticationUser)
        {
            string userAsJson = JsonSerializer.Serialize(inputAuthenticationUser);
            HttpContent content = new StringContent(userAsJson,
                Encoding.UTF8,
                "application/json");

            HttpResponseMessage response = await _client.PostAsync(
                Uri + "/api/Auth/login", content);

            return await response.Content.ReadAsStringAsync();

        }

        public async Task<List<User>> RetrieveAllUsers()
        { 
            HttpResponseMessage responseMessage = await _client.GetAsync(Uri + "/api/User");

            return await responseMessage.Content.ReadFromJsonAsync<List<User>>();

        }

    }
}