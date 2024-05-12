using System.Net.Http.Json;
using Microsoft.JSInterop;

namespace TrainingCenterUI.Services
{
    public class AdminService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;

        public AdminService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }

        public async Task<bool> Login(string email, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("/login", new { Email = email, Password = password });
            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
                if (tokenResponse?.Token != null)
                {
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", tokenResponse.Token);
                    return true;
                }
            }
            return false;
        }

        public class TokenResponse
        {
            public string Token { get; set; }
        }

    }
}
