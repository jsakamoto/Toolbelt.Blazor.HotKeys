using Microsoft.Extensions.DependencyInjection;
using Toolbelt.Blazor.HotKeys;

namespace Toolbelt.Blazor.Extensions
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
        public static IServiceCollection AddHotKeysBuilder(this IServiceCollection services)
        {
            services.AddSingleton<HotKeysBuilder>();
            return services;
        }
    }
}
