using System.Net.Http.Json;
using Microsoft.JSInterop;
using TrainingCenterUI.DTO;

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
            var loginRequest = new LoginRequest { Email = email, Password = password };
            var response = await _httpClient.PostAsJsonAsync("api/admin/login", loginRequest);
            Console.WriteLine(response.StatusCode);
            Console.WriteLine(response.Content);
            Console.WriteLine(response.Headers);
            Console.WriteLine(response.RequestMessage);
            Console.WriteLine(response.ReasonPhrase);
            Console.WriteLine(response.RequestMessage);
 
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
