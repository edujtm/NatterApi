using Npgsql;
using System.Data;
using AspNetCoreRateLimit;

using Natter.Api.Config;
using Natter.Api.Middleware;
using Natter.Api.Filters;
using Natter.Application;
using Natter.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var dbOptions = builder.Configuration.GetSection("Database").Get<DatabaseOptions>();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddMvc(options =>
{
    options.Filters.Add<ExceptionHandlerFilter>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

builder.Services.AddUseCases();
builder.Services.AddDbAccess(dbOptions.ConnectionString);
builder.Services.AddRepositories();

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSecurityHeaders();
//app.UseIpRateLimiting();
app.UseMiddleware<CustomIpRateLimiterMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
