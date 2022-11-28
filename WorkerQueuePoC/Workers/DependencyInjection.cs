namespace WorkerQueuePoC.Workers;

using WorkerQueuePoC.Workers.Users;

internal static class DependencyInjection
{
    public static IServiceCollection AddWorkers(this IServiceCollection services)
    {
        services.AddHostedService<CreateUsersWorker>();

        services.AddSingleton<CreateUsersQueue>();

        return services;
    }
}
