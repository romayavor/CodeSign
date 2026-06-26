using CodeSign.Shared;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace CodeSign.Client.Services;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly IJSRuntime _js;

    public AuthResponse? CurrentUser { get; private set; }
    public bool IsAuthenticated => CurrentUser != null;
    public bool IsAdmin => CurrentUser?.Role == "Admin";

    public event Action? OnAuthStateChanged;

    public AuthService(HttpClient http, IJSRuntime js)
    {
        _http = http;
        _js = js;
    }

    public async Task<bool> LoginAsync(LoginRequest request)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/Auth/login", request);
            if (!response.IsSuccessStatusCode) return false;

            var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
            if (authResponse == null) return false;

            CurrentUser = authResponse;

            // Зберігаємо токен в localStorage
            await _js.InvokeVoidAsync("localStorage.setItem", "authToken", authResponse.Token);
            await _js.InvokeVoidAsync("localStorage.setItem", "authUser",
                JsonSerializer.Serialize(authResponse));

            // Додаємо токен до всіх наступних запитів
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", authResponse.Token);

            OnAuthStateChanged?.Invoke();
            return true;
        }
        catch { return false; }
    }

    public async Task InitializeAsync()
    {
        try
        {
            var token = await _js.InvokeAsync<string?>(
                "localStorage.getItem", "authToken");
            var userJson = await _js.InvokeAsync<string?>(
                "localStorage.getItem", "authUser");

            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(userJson))
            {
                CurrentUser = JsonSerializer.Deserialize<AuthResponse>(userJson);
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }
        catch { }
    }

    public async Task LogoutAsync()
    {
        CurrentUser = null;
        _http.DefaultRequestHeaders.Authorization = null;
        await _js.InvokeVoidAsync("localStorage.removeItem", "authToken");
        await _js.InvokeVoidAsync("localStorage.removeItem", "authUser");
        OnAuthStateChanged?.Invoke();
    }
}