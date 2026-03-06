using CineBook.Blazor.Components;
using CineBook.Blazor.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthStateService>();
builder.Services.AddScoped<AuthenticationStateProvider, CineBookAuthStateProvider>();
builder.Services.AddScoped<CineBookAuthStateProvider>();

var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7100";

builder.Services.AddHttpClient<FilmApiClient>(client =>
    client.BaseAddress = new Uri(apiBaseUrl));

builder.Services.AddHttpClient<AuditoriumApiClient>(client =>
    client.BaseAddress = new Uri(apiBaseUrl));

builder.Services.AddHttpClient<ScreeningApiClient>(client =>
    client.BaseAddress = new Uri(apiBaseUrl));

builder.Services.AddHttpClient<ReservationApiClient>(client =>
    client.BaseAddress = new Uri(apiBaseUrl));

builder.Services.AddHttpClient<SeatApiClient>(client =>
    client.BaseAddress = new Uri(apiBaseUrl));

builder.Services.AddHttpClient<AuthApiClient>(client =>
    client.BaseAddress = new Uri(apiBaseUrl));

builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
