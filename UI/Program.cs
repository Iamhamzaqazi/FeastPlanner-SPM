global using AppDBContext.General;
global using AppDBContext.Interfaces.Cookies;
global using AppDBContext.Interfaces.MasterData;
global using AppDBContext.Interfaces.User;
global using AppDBContext.Models;
global using Blazored.LocalStorage;
global using MudBlazor;
global using MudBlazor.Services;
using AppDBContext.Interfaces.Authentication;
using AppDBContext.Interfaces.Booking;
using AppDBContext.Interfaces.Business;
using AppDBContext.Interfaces.Financials;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http.Connections;
using System.Text.Json.Serialization;
using UI.Authentication;
using UI.Services.Authenticate;
using UI.Services.Booking;
using UI.Services.Business;
using UI.Services.Financials;
using UI.Services.MasterData;
using UI.Services.User;

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
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Adjust as needed
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddScoped<ICookie, UI.Authentication.Cookie>();
builder.Services.AddScoped<IAuthenticate, AuthenticateService>();
builder.Services.AddScoped<IMstUser, MstUserService>();
builder.Services.AddScoped<IMstUserMessage, MstUserMessageService>();
builder.Services.AddScoped<ICfgUser, CfgUserService>();
builder.Services.AddScoped<IMstUserAuthorization, MstUserAuthorizationService>();
builder.Services.AddScoped<IMasterData, MasterDataService>();
builder.Services.AddScoped<IBusinessData, BusinessDataService>();
builder.Services.AddScoped<ITrnsFinancial, TrnsFinancialService>();
builder.Services.AddScoped<IBooking, BookingDataService>();
builder.Services.AddSingleton<ActiveUsersService>(); // Register active user service

UIConfig.AppVersion = builder.Configuration.GetValue<string>("AppVersion");
UIConfig.TotalProfileCompletion = builder.Configuration.GetValue<int>("TotalProfileCompletion");
UIConfig.TotalAccountCompletion = builder.Configuration.GetValue<int>("TotalAccountCompletion");
UIConfig.Option1 = builder.Configuration.GetValue<bool>("Option1");
UIConfig.APIBaseURL = builder.Configuration.GetValue<string>("APIBaseUrl");
UIConfig.ReportPath = builder.Configuration.GetValue<string>("ReportPath");
UIConfig.AttachmentPath = builder.Configuration.GetValue<string>("AttachmentPath");
UIConfig.CRBaseURL = builder.Configuration.GetValue<string>("CRBaseURL");
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
app.UseSession();
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
