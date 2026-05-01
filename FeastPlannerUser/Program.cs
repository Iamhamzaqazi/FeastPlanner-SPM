using AppDBContext.General;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Http.Connections;
using MudBlazor;
using MudBlazor.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddServerSideBlazor()
    .AddHubOptions(options =>
    {
        options.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
        options.HandshakeTimeout = TimeSpan.FromSeconds(30);
    });
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddSignalR();
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);
//SyncfusionLicenseProvider.RegisterLicense("NTc0MTUyQDMxMzkyZTM0MmUzMFJJSFl6c1pvL1lzRFdZSVJZblJsVGpHVCtmUTRiN3hNOW9qR0tJK1p5SlE9;NTc0MTUzQDMxMzkyZTM0MmUzMEhIVkdZUGVPZmlZYUhQZ3UvVEY0T3hzazlxeTRaOGRYYlBMTEZQdkZUV1U9");
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomCenter;

    config.SnackbarConfiguration.PreventDuplicates = true;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 5000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});


UIConfig.AppVersion = builder.Configuration.GetValue<string>("AppVersion");
UIConfig.APIBaseURL = builder.Configuration.GetValue<string>("APIBaseUrl");
UIConfig.NotificationBaseURL = builder.Configuration.GetValue<string>("NotificationBaseURL");
UIConfig.MessageBaseURL = builder.Configuration.GetValue<string>("MessageBaseURL");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub(configureOptions: options =>
{
    options.Transports = HttpTransportType.WebSockets | HttpTransportType.LongPolling;
});
app.MapFallbackToPage("/_Host");

app.Run();
