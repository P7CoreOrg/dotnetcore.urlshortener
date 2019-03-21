using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.PlatformAbstractions;

namespace UrlShortenerHost
{
    public class TestServerFixture : IDisposable
    {
        private readonly TestServer _testServer;
        public HttpClient Client { get; }

        public TestServerFixture()
        {
            var contentRootPath = GetContentRootPath();
            var builder = new WebHostBuilder()
                .UseContentRoot(contentRootPath)
                .UseEnvironment("Testing")
                .ConfigureServices(services =>
                {
                    services.TryAddTransient<IDefaultHttpClientFactory, TestDefaultHttpClientFactory>();
                })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var environmentName = hostingContext.HostingEnvironment.EnvironmentName;
                    config
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{environmentName}.json", optional: true);
                })
                .UseStartup<Startup>();  // Uses Start up class from your API Host project to configure the test server

            _testServer = new TestServer(builder);
            Client = _testServer.CreateClient();

        }

        private string GetContentRootPath()
        {
            var testProjectPath = PlatformServices.Default.Application.ApplicationBasePath;
            return testProjectPath;
        }

        public void Dispose()
        {
            Client.Dispose();
            _testServer.Dispose();
        }
        public TService GetService<TService>()
            where TService : class
        {
            return _testServer?.Host?.Services?.GetService(typeof(TService)) as TService;
        }
    }
}