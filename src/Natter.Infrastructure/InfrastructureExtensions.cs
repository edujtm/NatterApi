using System.Data;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Natter.Shared.Architecture;
using Natter.Infrastructure.DbAccess;

namespace Natter.Infrastructure;


public static class InfrastructureExtensions
{
    public static void AddDbAccess(this IServiceCollection services, string connString)
    {
        services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();
        services.AddScoped<IConnectionFactory, UnitOfWorkFactory>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IDbConnection>(_ => new NpgsqlConnection(connString));
    }
}