using System.Net.Http;
using Microsoft.AspNetCore.TestHost;

namespace UrlShortenerHost
{
    public class TestDefaultHttpClientFactory : IDefaultHttpClientFactory
    {
        public static TestServer TestServer { get; set; }
        public HttpMessageHandler HttpMessageHandler => TestServer.CreateHandler();
        public HttpClient HttpClient => TestServer.CreateClient();
    }
}