using System;
using System.Collections.Generic;
using System.Text;
using dotnetcore.urlshortener.contracts;
using Microsoft.Extensions.DependencyInjection;

namespace dotnetcore.urlshortener.InMemoryStore.Extensions
{
    public static class AspNetCoreExtensions
    {
        public static IServiceCollection AddInMemoryUrlShortenerConfiguration(this IServiceCollection services)
        {
            services.AddSingleton<IUrlShortenerConfiguration,InMemoryUrlShortenerConfiguration>(); // We must explicitly register Foo
            return services;
        }
        public static IServiceCollection AddInMemoryUrlShortenerStore(this IServiceCollection services)
        {
            services.AddSingleton<InMemoryUrlShortenerStore>(); // We must explicitly register Foo
            services.AddSingleton<IUrlShortenerStore>(x => x.GetRequiredService<InMemoryUrlShortenerStore>()); // Forward requests to Foo
            services.AddSingleton<IUrlShortenerEventSource<ShortenerEventArgs>>(x => x.GetRequiredService<InMemoryUrlShortenerStore>()); // Forward requests to Foo
            return services;
        }
    }
}
