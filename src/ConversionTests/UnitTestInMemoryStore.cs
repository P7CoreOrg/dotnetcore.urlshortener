using System;
using System.Threading.Tasks;
using dotnetcore.urlshortener.contracts;
using dotnetcore.urlshortener.generator;
using dotnetcore.urlshortener.InMemoryStore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using ShortUrl = dotnetcore.urlshortener.contracts.ShortUrl;

namespace ConversionTests
{
    [TestClass]
    public class UnitTestInMemoryStore
    {
        [TestMethod]
        public async Task TestMethod_StoreUrl_GetUrl()
        {
            var url = "https://github.com/P7CoreOrg/dotnetcore.urlshortener/tree/dev";
            var store = new InMemoryUrlShortenerStore();
            var shortUrl = await store.UpsertShortUrlAsync(new ShortUrl()
            {
                LongUrl = url,
                Exiration = DateTime.UtcNow.AddDays(1)

            });
            shortUrl.LongUrl.ShouldMatch(url);
            shortUrl.Id.ShouldNotBeNullOrEmpty();

            var lookup = await store.GetShortUrlAsync(shortUrl.Id);
            shortUrl.Id.ShouldMatch(lookup.Id);
            shortUrl.LongUrl.ShouldMatch(lookup.LongUrl);
        }
        [TestMethod]
        public async Task TestMethod_StoreUrl_RemoveUrl()
        {
            var url = "https://github.com/P7CoreOrg/dotnetcore.urlshortener/tree/dev";
            var store = new InMemoryUrlShortenerStore();
            var shortUrl = await store.UpsertShortUrlAsync(new ShortUrl()
            {
                LongUrl = url,
                Exiration = DateTime.UtcNow.AddDays(1)

            });
            shortUrl.LongUrl.ShouldMatch(url);
            shortUrl.Id.ShouldNotBeNullOrEmpty();

            var lookup = await store.GetShortUrlAsync(shortUrl.Id);
            shortUrl.Id.ShouldMatch(lookup.Id);
            shortUrl.LongUrl.ShouldMatch(lookup.LongUrl);

            await store.RemoveShortUrlAsync(shortUrl.Id);
            lookup = await store.GetShortUrlAsync(shortUrl.Id);
            lookup.ShouldBeNull();
        }

        [TestMethod]
        public async Task TestMethod_StoreUrl_Expiration()
        {
            var url = "https://github.com/P7CoreOrg/dotnetcore.urlshortener/tree/dev";
            var store = new InMemoryUrlShortenerStore();
            var shortUrl = await store.UpsertShortUrlAsync(new ShortUrl()
            {
                LongUrl = url,
                Exiration = DateTime.UtcNow

            });
            shortUrl.LongUrl.ShouldMatch(url);
            shortUrl.Id.ShouldNotBeNullOrEmpty();

            var lookup = await store.GetShortUrlAsync(shortUrl.Id);
            lookup.ShouldBeNull();
        }

    }
}