// Had to create this project because this FluentMigrator CLI
// does not currently support .NET 6
using System.Reflection;
using FluentMigrator;
using FluentMigrator.Runner;

using Microsoft.Extensions.DependencyInjection;

namespace Natter.Migrations;

public class Program
{
    static ServiceProvider CreateServices(string connString)
    {
        return new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(rb =>
            {
                rb.AddPostgres()
                    .WithGlobalConnectionString(connString)
                    .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations();
            })
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .BuildServiceProvider(false);
    }

    static void RunMigrations(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        runner.MigrateUp();
    }

    static void Main(string[] args)
    {
        var connString = Environment.GetCommandLineArgs()[1];

        using var serviceProvider = CreateServices(connString);
        using var scope = serviceProvider.CreateScope();

        RunMigrations(scope.ServiceProvider);
    }

}