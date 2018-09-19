using Microsoft.AspNetCore.Blazor.Builder;
using Microsoft.Extensions.DependencyInjection;
using Toolbelt.Blazor.Extensions;

namespace SampleSite
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHotKeys();
        }

        public void Configure(IBlazorApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
