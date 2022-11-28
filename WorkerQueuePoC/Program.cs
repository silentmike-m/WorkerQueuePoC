using System.Reflection;
using MediatR;
using WorkerQueuePoC.Workers;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddWorkers();
    })
    .Build();

await host.RunAsync();
