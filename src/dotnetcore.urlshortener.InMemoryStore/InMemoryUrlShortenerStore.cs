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
                return _database[id];
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
