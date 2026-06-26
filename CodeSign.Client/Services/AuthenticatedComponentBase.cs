using CodeSign.Client.Services;
using Microsoft.AspNetCore.Components;

namespace CodeSign.Client.Services;

public class AuthenticatedComponentBase : ComponentBase
{
    [Inject] protected AuthService AuthService { get; set; } = null!;
    [Inject] protected NavigationManager Navigation { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        await AuthService.InitializeAsync();

        if (!AuthService.IsAuthenticated)
        {
            Navigation.NavigateTo("/login");
            return;
        }

        await OnAuthenticatedAsync();
    }

    protected virtual Task OnAuthenticatedAsync() => Task.CompletedTask;
}