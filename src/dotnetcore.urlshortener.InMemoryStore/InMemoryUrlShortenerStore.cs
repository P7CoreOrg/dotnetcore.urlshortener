using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using dotnetcore.urlshortener.contracts;
using dotnetcore.urlshortener.generator;
using dotnetcore.urlshortener.generator.Extensions;
using ShortUrl = dotnetcore.urlshortener.contracts.ShortUrl;

namespace dotnetcore.urlshortener.InMemoryStore
{
    public class InMemoryUrlShortenerConfiguration : IUrlShortenerConfiguration
    {
        private Dictionary<string, ExpirationRedirectRecord> _database;

        public InMemoryUrlShortenerConfiguration()
        {
            _database = new Dictionary<string, ExpirationRedirectRecord>();
        }
        public async Task<ExpirationRedirectRecord> GetExpirationRedirectRecordAsync(string key)
        {
            throw new NotImplementedException();
        }
    }
    public class InMemoryUrlShortenerStore : IUrlShortenerStore
    {
        private EventSource<ShortenerEventArgs> _eventSource;
        public event EventHandler ThresholdReached;
        private Dictionary<string, ShortUrl> _database;
        private IUrlShortenerConfiguration _urlShortenerConfiguration;

        void IUrlShortenerEventSource<ShortenerEventArgs>.AddListenter(EventHandler<ShortenerEventArgs> handler)
        {
            _eventSource.AddListenter(handler);
        }

        void IUrlShortenerEventSource<ShortenerEventArgs>.RemoveListenter(EventHandler<ShortenerEventArgs> handler)
        {
            _eventSource.RemoveListenter(handler);
        }

       
        public InMemoryUrlShortenerStore(IUrlShortenerConfiguration urlShortenerConfiguration = null)
        {
            _urlShortenerConfiguration = urlShortenerConfiguration;
            _eventSource = new EventSource<ShortenerEventArgs>();
            _database = new Dictionary<string, ShortUrl>();
        }
        public async Task<ShortUrl> UpsertShortUrlAsync(ShortUrl shortUrl)
        {
            Guard.ArgumentNotNull(nameof(shortUrl),shortUrl);
            Guard.ArgumentNotNullOrEmpty(nameof(shortUrl.LongUrl),shortUrl.LongUrl);
            Guard.ArgumentNotNull(nameof(shortUrl.Exiration), shortUrl.Exiration);

            if (string.IsNullOrEmpty(shortUrl.ExpiredRedirectKey))
            {
                shortUrl.ExpiredRedirectKey = "0000";
            }
            else
            {
                Guard.ArguementEvalutate(nameof(shortUrl.ExpiredRedirectKey),
                    (() =>
                    {
                        if (shortUrl.ExpiredRedirectKey.Length != 4)
                        {
                            return (false, "The value must be exactly 4 in length");
                        }

                        return (true, null);
                    }));
                Guard.ArguementEvalutate(nameof(shortUrl.ExpiredRedirectKey),
                    (() =>
                    {
                        Regex r = new Regex("^[a-zA-Z0-9]*$");
                        if (!r.IsMatch(shortUrl.ExpiredRedirectKey))
                        {
                            return (false, "The value must be an alphanumeric");
                        }
                        return (true, null);
                    }));
            }

            var guid = Guid.NewGuid();
            var shortId = guid.ToShortBase64();
            shortUrl.Id = $"{shortUrl.ExpiredRedirectKey}{shortId}";
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
            Guard.ArguementEvalutate(nameof(id),
                (() =>
                {
                    if (id.Length <= 4)
                    {
                        return (false, "The value must be greater than 4 in length");
                    }

                    return (true, null);
                }));
            var expireRedirectKey = id.Substring(0, 4);
            var key = id.Substring(4);
            if (_database.ContainsKey(key))
            {
                var record = _database[key];
                if (record.Exiration <= DateTime.UtcNow)
                {
                    _database.Remove(id);
                    _eventSource.FireEvent(new ShortenerEventArgs()
                    {
                        ShortUrl = record,
                        EventType = ShortenerEventType.Expired,
                        UtcDateTime = DateTime.UtcNow
                    });
                    //TODO: lookup redirect key and send that back
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
            Guard.ArguementEvalutate(nameof(id),
                (() =>
                {
                    if (id.Length <= 4)
                    {
                        return (false, "The value must be greater than 4 in length");
                    }

                    return (true, null);
                }));
            var expireRedirectKey = id.Substring(0, 4);
            var key = id.Substring(4);
            if (_database.ContainsKey(key))
            {
                _eventSource.FireEvent(new ShortenerEventArgs()
                {
                    ShortUrl = _database[key],
                    EventType = ShortenerEventType.Remove,
                    UtcDateTime = DateTime.UtcNow
                });
                _database.Remove(key);
            }
        }

       
    }
}
