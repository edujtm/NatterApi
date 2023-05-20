using Npgsql;
using System.Data;

using Natter.Api.Config;
using Natter.Application;
using Natter.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var dbOptions = builder.Configuration.GetSection("Database").Get<DatabaseOptions>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
