using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dotnetcore.urlshortener.contracts;
using dotnetcore.urlshortener.generator;
using dotnetcore.urlshortener.generator.Extensions;
using ShortUrl = dotnetcore.urlshortener.contracts.ShortUrl;

namespace dotnetcore.urlshortener.InMemoryStore
{
    public class InMemoryUrlShortenerStore : IUrlShortenerStore
    {
        private Dictionary<string, ShortUrl> _database;

        public InMemoryUrlShortenerStore()
        {
            _database = new Dictionary<string, ShortUrl>();
        }
        public async Task<ShortUrl> UpsertShortUrlAsync(ShortUrl shortUrl)
        {
            Guard.ArgumentNotNull(nameof(shortUrl),shortUrl);
            Guard.ArgumentNotNullOrEmpty(nameof(shortUrl.LongUrl),shortUrl.LongUrl);
            Guard.ArgumentNotNull(nameof(shortUrl.Exiration), shortUrl.Exiration);


            var guid = Guid.NewGuid();
            var shortId = guid.ToShortBase64();
            shortUrl.Id = shortId;
            _database.Add(shortId, shortUrl);
            return shortUrl;
        }

        public async Task<ShortUrl> GetShortUrlAsync(string id)
        {
            if (_database.ContainsKey(id))
            {
                var record = _database[id];
                if (record.Exiration <= DateTime.UtcNow)
                {
                    _database.Remove(id);
                    return null;
                }

                return record;
            }

            return null;
          
        }

        public async Task RemoveShortUrlAsync(string id)
        {
            if (_database.ContainsKey(id))
            {
                _database.Remove(id);
            }
        }
    }
}
