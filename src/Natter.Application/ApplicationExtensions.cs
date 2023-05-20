using Microsoft.Extensions.DependencyInjection;

using Scrutor;

using Natter.Shared.Architecture;

namespace Natter.Application;

public static class ApplicationExtensions
{
    public static void AddUseCases(this IServiceCollection services)
    {
        services.Scan(scan =>
        {
            scan.FromAssemblyOf<ApplicationExtensions.Ref>()
                .AddClasses(classes => classes.AssignableTo<IUseCase>())
                .AsSelf()
                .WithTransientLifetime();
        });
    }

    public class Ref { }
}