using System.Net.Http.Json;
using Microsoft.JSInterop;
using System.Threading.Tasks;

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

        public async Task<bool> ValidateToken(string token)
        {
            var response = await _httpClient.GetAsync($"api/admin/validateToken?token={token}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Login(string email, string password)
        {
            var loginRequest = new LoginRequest { Email = email, Password = password };
            var response = await _httpClient.PostAsJsonAsync("api/admin/login", loginRequest);

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

        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}
