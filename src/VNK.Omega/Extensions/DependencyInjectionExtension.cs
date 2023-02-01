using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using VNK.Omega.BackgroundServices;
using VNK.Omega.Interfaces;
using VNK.Omega.Logger;
using VNK.Omega.Options;
using VNK.Omega.Scheduler;
using VNK.Omega.Workers;

namespace VNK.Omega.Extensions;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddOmega(this IServiceCollection service, Action<OmegaServerSettings>? setup = null)
    {
        service.AddTransient<IConfigureOptions<OmegaServerSettings>, OmegaServerConfigure>();
        if (setup != null) service.Configure(setup);
        service.AddSingleton<IOmegaLogger, OmegaLogger>();
        service.AddHostedService<HostedService>();
        service.AddSingleton<ITaskQueue, TaskQueue>();

        return service;
    }

    public static IServiceCollection AddWorker<T>(this IServiceCollection service) where T : Worker
    {
        service.AddTransient<IWorker, T>();
        
        return service;
    }
}