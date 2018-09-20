﻿using Microsoft.Extensions.DependencyInjection;

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
            services.AddSingleton(_ => new global::Toolbelt.Blazor.HotKeys.HotKeys());
            return services;
        }
    }
}
