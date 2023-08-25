namespace Natter.Infrastructure;
using System.Data.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Natter.Domain.Entities;
using Natter.Domain.Repositories;
using Natter.Infrastructure.DbAccess;
using Natter.Infrastructure.DbAccess.Repositories;
using Natter.Infrastructure.Identity;
using Natter.Shared.Architecture;
using Npgsql;


public static class InfrastructureExtensions
{
    public static void AddDbAccess(this IServiceCollection services, string connString)
    {
        // This needs to be done in order to register a single scoped instance with multiple interfaces,
        // which is necessary for the correct interaction between the UnitOfWork and the repositories.
        //
        // Registering the UnitOfWorkFactory the following way would be wrong:
        //      services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();
        //      services.AddScoped<IConnectionFactory, UnitOfWorkFactory>();
        // Because two different instances would be created for the same scope (one for each interface).
        services.AddScoped<UnitOfWorkFactory>();
        services.AddScoped<IUnitOfWorkFactory>(services => services.GetRequiredService<UnitOfWorkFactory>());
        services.AddScoped<IConnectionFactory>(services => services.GetRequiredService<UnitOfWorkFactory>());

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<DbConnection>(_ =>
        {
            var connection = new NpgsqlConnection(connString);
            connection.Open();
            return connection;
        });
    }

    public static void AddRepositories(this IServiceCollection services) => services.AddTransient<ISpaceRepository, SpaceRepository>();

    public static void AddIdentityServices(this IServiceCollection services)
    {
        services.AddIdentityCore<NatterUser>(options => { });
        services.AddScoped<IUserStore<NatterUser>, NatterUserStore>();
    }
}
