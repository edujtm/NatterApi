namespace Natter.Application;
using Microsoft.Extensions.DependencyInjection;
using Natter.Shared.Architecture;

public static class ApplicationExtensions
{
    public static void AddUseCases(this IServiceCollection services) => services.Scan(scan => scan.FromAssemblyOf<Ref>()
                                                                              .AddClasses(classes => classes.AssignableTo<IUseCase>())
                                                                              .AsSelf()
                                                                              .WithTransientLifetime());

    public class Ref { }
}
