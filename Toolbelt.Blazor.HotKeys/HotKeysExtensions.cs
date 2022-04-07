using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using Toolbelt.Blazor.HotKeys;

namespace Toolbelt.Blazor.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for adding HotKeys service.
    /// </summary>
    public static class HotKeysExtensions
    {
        /// <summary>
        ///  Adds a HotKeys service to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add the service to.</param>
        public static IServiceCollection AddHotKeys(this IServiceCollection services)
        {
            return services.AddHotKeys(configure: null);
        }

        /// <summary>
        ///  Adds a HotKeys service to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add the service to.</param>
        /// <param name="configure">An <see cref="System.Action"/> to configure the options for HotKeys service.</param>
        public static IServiceCollection AddHotKeys(this IServiceCollection services, Action<HotKeysOptions> configure)
        {
            return services.AddScoped(serviceProvider =>
            {
                var options = new HotKeysOptions();
                configure?.Invoke(options);
                var jsRuntime = serviceProvider.GetRequiredService<IJSRuntime>();
                var logger = serviceProvider.GetRequiredService<ILogger<global::Toolbelt.Blazor.HotKeys.HotKeys>>();
                return new global::Toolbelt.Blazor.HotKeys.HotKeys(jsRuntime, options, logger);
            });
        }
    }
}
