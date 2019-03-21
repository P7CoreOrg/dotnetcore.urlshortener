using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dotnetcore.urlshortener.contracts;
using Microsoft.Extensions.Configuration;

namespace dotnetcore.urlshortener.InMemoryStore
{
    public class InMemoryUrlShortenerConfigurationModel
    {
        public List<ExpirationRedirectRecord> Records { get; set; }
        public string DefaultExpiredRedirectKey { get; set; }
    }
    public class InMemoryUrlShortenerConfiguration : IUrlShortenerConfiguration
    {
        private Dictionary<string, ExpirationRedirectRecord> _database;
       private string DefaultExpiredRedirectKey { get; set; }
        public InMemoryUrlShortenerConfiguration(IConfiguration configuration )
        {
            _database = new Dictionary<string, ExpirationRedirectRecord>();
            var section = configuration.GetSection("inMemoryUrlShortenerConfiguration");
            var model = new InMemoryUrlShortenerConfigurationModel();
           
            section.Bind(model);

            foreach (var record in model.Records)
            {
                _database[record.ExpiredRedirectKey] = record;
            }

            DefaultExpiredRedirectKey = model.DefaultExpiredRedirectKey;
        }
        public async Task<ExpirationRedirectRecord> GetExpirationRedirectRecordAsync(string key)
        {
            if (_database.ContainsKey(key))
            {
                return _database[key];
            }

            if (_database.ContainsKey(DefaultExpiredRedirectKey))
            {
                return _database[DefaultExpiredRedirectKey];
            }
            return null;
        }
    }
}