using Gcds.Net.Renderer;

using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Gcds.Net.Services
{
    public static class ServiceCollectionExtensions
    {
        public static void UseGcDesignServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.TryAdd(ServiceDescriptor.Scoped<IGcDesignModelAccessor, GcDesignModelAccessor>());
            services.AddTransient<IGcdsRenderer, GcdsRenderer>();
            services.AddTransient<IHtmlGenerator, GcdsHtmlGenerator>();
        }
    }
}
