using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace Toolbelt.Blazor.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for adding Hoteys service.
    /// </summary>
    public static class HotKeysExtensions
    {
        /// <summary>
        ///  Adds a HotKeys service to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add the service to.</param>
        public static IServiceCollection AddHotKeys(this IServiceCollection services)
        {
            services.AddScoped(serviceProvider => new global::Toolbelt.Blazor.HotKeys.HotKeys(serviceProvider.GetService<IJSRuntime>()));
            return services;
        }
    }
}
