using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JWTBlazorApp.Data;
using JWTBlazorApp.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace JWTBlazorApp.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        
        private const string Uri = "https://localhost:7166";
        
        private readonly HttpClient _client;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ISessionStorageService _sessionStorage;

        public AuthenticationService(HttpClient client,
            AuthenticationStateProvider authStateProvider,
            ISessionStorageService sessionStorage)
        {
            _client = client;
            _authStateProvider = authStateProvider;
            _sessionStorage = sessionStorage;
        }

        public async Task<AuthenticatedUserModel> Login(AuthenticationUserModel userForAuthentication)
        {
            string userAsJson = JsonSerializer.Serialize(userForAuthentication);
            HttpContent content = new StringContent(userAsJson,
                Encoding.UTF8,
                "application/json");
            
            var authResult = await _client.PostAsync($"{Uri}/login", content);
            
            if (authResult.IsSuccessStatusCode is false)
            {
                return null;
            }

            AuthenticatedUserModel result = await authResult.Content.ReadFromJsonAsync<AuthenticatedUserModel>() ?? throw new Exception("User not found");
            

            await _sessionStorage.SetItemAsync("authToken", result.AccessToken);
            
            Console.WriteLine("TOKEN: " + result.AccessToken);
            
            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.AccessToken);

            _client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue(
                    "bearer", 
                    result.AccessToken);
            return result;
        }
        
        public async Task<string> GetHelloMessage()
        {
            _client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue(
                    "bearer", 
                    await _sessionStorage.GetItemAsync<string>("authToken"));
            
            
            var authResult = await _client.GetAsync("https://localhost:7166/api/Message");
            if (!authResult.IsSuccessStatusCode) return authResult.StatusCode.ToString();
            string message = await authResult.Content.ReadAsStringAsync();
            return message;

        }

        public async Task Logout()
        {
            await _sessionStorage.RemoveItemAsync("authToken");
            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
            _client.DefaultRequestHeaders.Authorization = null;
        }
    }
    
    
}