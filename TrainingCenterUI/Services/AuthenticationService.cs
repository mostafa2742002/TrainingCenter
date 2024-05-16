using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace TrainingCenterUI.Services
{
    public class AuthenticationService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly AdminService _adminService;

        public AuthenticationService(IJSRuntime jsRuntime, AdminService adminService)
        {
            _jsRuntime = jsRuntime;
            _adminService = adminService;
        }

        public bool IsAuthenticated { get; private set; }

        public async Task<bool> Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            bool loginResponse = await _adminService.Login(email, password);
            if (loginResponse)
            {
                IsAuthenticated = true;
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "isAuthenticated", "true");
                return true;
            }

            return IsAuthenticated;
        }

        public async Task Initialize()
        {
            var token = await GetTokenAsync();
            IsAuthenticated = !string.IsNullOrEmpty(token) && !IsTokenExpired(token);
        }

        public async Task Logout()
        {
            IsAuthenticated = false;
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "isAuthenticated");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
        }

        public bool IsTokenExpired(string token)
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
            return jwtToken == null || jwtToken.ValidTo < DateTime.UtcNow;
        }

        public async Task<string> GetTokenAsync()
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
        }
    }
}
