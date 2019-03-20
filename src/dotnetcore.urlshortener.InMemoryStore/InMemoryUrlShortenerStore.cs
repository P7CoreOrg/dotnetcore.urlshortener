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
        private EventSource<ShortenerEventArgs> _eventSource;
        public event EventHandler ThresholdReached;
        private Dictionary<string, ShortUrl> _database;

        void IUrlShortenerEventSource<ShortenerEventArgs>.AddListenter(EventHandler<ShortenerEventArgs> handler)
        {
            _eventSource.AddListenter(handler);
        }

        void IUrlShortenerEventSource<ShortenerEventArgs>.RemoveListenter(EventHandler<ShortenerEventArgs> handler)
        {
            _eventSource.RemoveListenter(handler);
        }

       
        public InMemoryUrlShortenerStore()
        {
            _eventSource = new EventSource<ShortenerEventArgs>();
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
            _eventSource.FireEvent(new ShortenerEventArgs()
            {
                ShortUrl = shortUrl,
                EventType = ShortenerEventType.Upsert,
                UtcDateTime = DateTime.UtcNow
            });
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
                _eventSource.FireEvent(new ShortenerEventArgs()
                {
                    ShortUrl = record,
                    EventType = ShortenerEventType.Get,
                    UtcDateTime = DateTime.UtcNow
                });
                return record;
            }

            return null;
          
        }

        public async Task RemoveShortUrlAsync(string id)
        {
            if (_database.ContainsKey(id))
            {
                _eventSource.FireEvent(new ShortenerEventArgs()
                {
                    ShortUrl = _database[id],
                    EventType = ShortenerEventType.Remove,
                    UtcDateTime = DateTime.UtcNow
                });
                _database.Remove(id);
            }
        }

       
    }
}
