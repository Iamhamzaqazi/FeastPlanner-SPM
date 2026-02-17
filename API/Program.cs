using API.Authentication;
using API.DapperDAL;
using API.Repository.Authenticate;
using API.Repository.Business;
using API.Repository.Financials;
using API.Repository.MasterData;
using API.Repository.User;
using API.SignalHub;
using AppDBContext.General;
using AppDBContext.Interfaces.Authentication;
using AppDBContext.Interfaces.Business;
using AppDBContext.Interfaces.Dapper;
using AppDBContext.Interfaces.Financials;
using AppDBContext.Interfaces.MasterData;
using AppDBContext.Interfaces.User;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddSignalR();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);

builder.Services.AddTransient<ITokenManager, TokenManager>();
builder.Services.AddScoped<IDapper, DapperClass>();
builder.Services.AddScoped<IAuthenticate, AuthenticateRepo>();
builder.Services.AddScoped<IMstUser, MstUserRepo>();
builder.Services.AddScoped<IMstUserMessage, MstUserMessageRepo>();
builder.Services.AddScoped<ICfgUser, CfgUserRepo>();
builder.Services.AddScoped<IMstUserAuthorization, MstUserAuthorizationRepo>();
builder.Services.AddScoped<IMasterData, MasterDataRepo>();
builder.Services.AddScoped<IBusinessData, BusinessDataRepo>();
builder.Services.AddScoped<ITrnsFinancial, TrnsFinancialRepo>();

APIConfig.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
APIConfig.RepositoryType = builder.Configuration.GetValue<string>("RepositoryType");
APIConfig.TitleConfig = builder.Configuration.GetValue<string>("TitleConfig");
APIConfig.EmailConfig = builder.Configuration.GetValue<string>("EmailConfig");
APIConfig.PasswordConfig = builder.Configuration.GetValue<string>("PasswordConfig");
APIConfig.HostConfig = builder.Configuration.GetValue<string>("HostConfig");
APIConfig.PortConfig = builder.Configuration.GetValue<int>("PortConfig");
APIConfig.IsSSlConfig = builder.Configuration.GetValue<bool>("IsSSlConfig");
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHub<NotificationHub>("/notification_hub");

app.Run();
