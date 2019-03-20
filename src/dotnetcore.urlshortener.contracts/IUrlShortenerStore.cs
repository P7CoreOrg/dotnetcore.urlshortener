using System;
using System.Threading.Tasks;

namespace dotnetcore.urlshortener.contracts
{
    public class ShortUrl
    {
        public string LongUrl { get; set; }
        public string Id { get; set; }
    }
    public interface IUrlShortenerStore
    {
        Task<ShortUrl> UpsertShortUrlAsync(ShortUrl shortUrl);
        Task<ShortUrl> GetShortUrlAsync(string id);
        Task RemoveShortUrlAsync(string id);
    }
}
