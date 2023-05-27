// Had to create this project because this FluentMigrator CLI
// does not currently support .NET 6
namespace Natter.Migrations;
using System.Reflection;
using FluentMigrator.Runner;

using Microsoft.Extensions.DependencyInjection;

public class Program
{
    private static ServiceProvider CreateServices(string connString) => new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb.AddPostgres()
                    .WithGlobalConnectionString(connString)
                    .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .BuildServiceProvider(false);

    private static void RunMigrations(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        runner.MigrateUp();
    }

    private static void Main(string[] args)
    {
        var connString = Environment.GetCommandLineArgs()[1];

        using var serviceProvider = CreateServices(connString);
        using var scope = serviceProvider.CreateScope();

        RunMigrations(scope.ServiceProvider);
    }

}
