using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace TaskBoard.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        services.AddAutoMapper(_ => { }, Assembly.GetExecutingAssembly());


        return services;
    }
}
