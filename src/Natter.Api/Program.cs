using Npgsql;
using System.Data;
using Natter.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddUseCases();

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});


// TODO: move to infrastructure project
var connString = builder.Configuration.GetConnectionString("DbConn");
builder.Services.AddScoped<IDbConnection>(_ => new NpgsqlConnection(connString));

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
