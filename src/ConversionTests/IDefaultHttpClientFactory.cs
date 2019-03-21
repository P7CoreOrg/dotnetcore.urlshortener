using System.Net.Http;

namespace UrlShortenerHost
{
    public interface IDefaultHttpClientFactory
    {
        HttpMessageHandler HttpMessageHandler { get; }
        HttpClient HttpClient { get; }
    }
}