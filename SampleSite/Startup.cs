using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace SampleSite
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHotKeys();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
