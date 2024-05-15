using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace TrainingCenterUI.Services
{
    public class AuthenticationService
    {
        private readonly IJSRuntime _jsRuntime;
        public bool IsAuthenticated { get; private set; }

        public AuthenticationService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<bool> Authenticate(string email, string password, AdminService adminService)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            bool loginResponse = await adminService.Login(email, password);
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
            var isAuthenticated = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "isAuthenticated");
            IsAuthenticated = isAuthenticated == "true";
        }

        public async Task Logout()
        {
            IsAuthenticated = false;
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "isAuthenticated");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
        }
    }
}
