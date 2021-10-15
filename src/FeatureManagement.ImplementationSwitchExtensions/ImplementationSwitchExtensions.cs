using FeatureManagement.ImplementationSwitchExtensions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.FeatureManagement;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ImplementationSwitchExtensions
    {
        public static IServiceCollection AddSingletonFeature<TService, TFeatureOnImplementation, TFeatureOffImplementation>
            (this IServiceCollection services, string featureName)
            where TService : class
            where TFeatureOnImplementation : class, TService
            where TFeatureOffImplementation : class, TService
        {
            TryAdd<TService, TFeatureOnImplementation, TFeatureOffImplementation, IFeatureManager>(
                services, featureName, ServiceLifetime.Singleton);

            return services;
        }

        public static IServiceCollection AddScopedFeature<TService, TFeatureOnImplementation, TFeatureOffImplementation>
            (this IServiceCollection services, string featureName)
            where TService : class
            where TFeatureOnImplementation : class, TService
            where TFeatureOffImplementation : class, TService
        {
            TryAdd<TService, TFeatureOnImplementation, TFeatureOffImplementation, IFeatureManagerSnapshot>(
                services, featureName, ServiceLifetime.Scoped);

            return services;
        }

        public static IServiceCollection AddTransientFeature<TService, TFeatureOnImplementation, TFeatureOffImplementation>
            (this IServiceCollection services, string featureName)
            where TService : class
            where TFeatureOnImplementation : class, TService
            where TFeatureOffImplementation : class, TService
        {
            TryAdd<TService, TFeatureOnImplementation, TFeatureOffImplementation, IFeatureManagerSnapshot>(
                services, featureName, ServiceLifetime.Transient);

            return services;
        }

        private static IServiceCollection TryAdd<TService, TFeatureImplementationOn, TFeatureImplementationOff,
            TFeatureManager>
            (
            IServiceCollection collection,
            string featureName,
            ServiceLifetime lifetime)
            where TService : class
            where TFeatureImplementationOn : class, TService
            where TFeatureImplementationOff : class, TService
            where TFeatureManager : IFeatureManager
        {
            collection.TryAddSingleton<IImplementationSwitchSettings<TService>>(
                new ImplementationSwitchSettings<TService, TFeatureImplementationOn, TFeatureImplementationOff>(featureName));

            var descriptor = new ServiceDescriptor(
                typeof(TService),
                serviceProvider =>
                {
                    var settings = serviceProvider.GetService<IImplementationSwitchSettings<TService>>();
                    var featureManagerSnapshot = serviceProvider.GetRequiredService<TFeatureManager>();
                    var task = featureManagerSnapshot.IsEnabledAsync(settings.FeatureName);

                    // [EN] It isn't good when settings are read from an asynchronous source,
                    // but not relevant to settings stored in configuration files.
                    // [RU] Это нехорошо, когда настройки читаются из асинхронного источника,
                    // но несущественно для настроек, хранимых в конфигурационных файлах.
                    bool result = task.GetAwaiter().GetResult();

                    if (result)
                        return serviceProvider.GetRequiredService<TFeatureImplementationOn>();
                    else
                        return serviceProvider.GetRequiredService<TFeatureImplementationOff>();
                },
                lifetime);
            collection.Add(descriptor);

            descriptor = new ServiceDescriptor(
                typeof(TFeatureImplementationOn), typeof(TFeatureImplementationOn), lifetime);
            collection.TryAdd(descriptor);

            descriptor = new ServiceDescriptor(
                typeof(TFeatureImplementationOff), typeof(TFeatureImplementationOff), lifetime);
            collection.TryAdd(descriptor);
            return collection;
        }
    }
}
