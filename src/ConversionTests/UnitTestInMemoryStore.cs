using System;
using System.Threading.Tasks;
using dotnetcore.urlshortener.contracts;
using dotnetcore.urlshortener.generator;
using dotnetcore.urlshortener.InMemoryStore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using UrlShortenerHost;
using ShortUrl = dotnetcore.urlshortener.contracts.ShortUrl;

namespace ConversionTests
{
    public class MyHandler
    {
        public ShortenerEventArgs Evt { get; set; }
        public MyHandler()
        {
        }

        public void OnEvent(object sender, ShortenerEventArgs e)
        {
            Evt = e;
        }
    }
    [TestClass]
    public class UnitTestInMemoryStore
    {
        private TestServerFixture _testServerFixture;

        [TestInitialize]
        public async Task Initialize()
        {
            _testServerFixture = new TestServerFixture();
        }
        [TestMethod]
        public async Task TestMethod_StoreUrl_AddRemoveEventHandler()
        {
            var store = _testServerFixture.GetService<IUrlShortenerStore>();
            var url = "https://github.com/P7CoreOrg/dotnetcore.urlshortener/tree/dev";
           
            ShortenerEventArgs evt = null;
            var myHandler = new MyHandler();
            store.AddListenter(myHandler.OnEvent);
            var shortUrl = await store.UpsertShortUrlAsync(new ShortUrl()
            {
                LongUrl = url,
                Exiration = DateTime.UtcNow.AddDays(1)

            });
            myHandler.Evt.ShouldNotBeNull();
            myHandler.Evt.EventType.ShouldBe(ShortenerEventType.Upsert);
            myHandler.Evt.ShortUrl.ShouldNotBeNull();
            myHandler.Evt.ShortUrl.LongUrl.ShouldMatch(url);
            myHandler.Evt.ShortUrl.Id.ShouldNotBeNullOrEmpty();

            shortUrl.LongUrl.ShouldMatch(url);
            shortUrl.Id.ShouldNotBeNullOrEmpty();

            myHandler.Evt = null;
            store.RemoveListenter(myHandler.OnEvent);

            var lookup = await store.GetShortUrlAsync(shortUrl.Id);
            myHandler.Evt.ShouldBeNull();
        }


        [TestMethod]
        public async Task TestMethod_StoreUrl_GetUrl()
        {
            var url = "https://github.com/P7CoreOrg/dotnetcore.urlshortener/tree/dev";
            var store = _testServerFixture.GetService<IUrlShortenerStore>();
            ShortenerEventArgs evt = null;
            var myHandler = new MyHandler();
            store.AddListenter(myHandler.OnEvent);
            var shortUrl = await store.UpsertShortUrlAsync(new ShortUrl()
            {
                LongUrl = url,
                Exiration = DateTime.UtcNow.AddDays(1)

            });
            myHandler.Evt.ShouldNotBeNull();
            myHandler.Evt.EventType.ShouldBe(ShortenerEventType.Upsert);
            myHandler.Evt.ShortUrl.ShouldNotBeNull();
            myHandler.Evt.ShortUrl.LongUrl.ShouldMatch(url);
            myHandler.Evt.ShortUrl.Id.ShouldNotBeNullOrEmpty();

            shortUrl.LongUrl.ShouldMatch(url);
            shortUrl.Id.ShouldNotBeNullOrEmpty();

            myHandler.Evt = null;
            var lookup = await store.GetShortUrlAsync(shortUrl.Id);
            myHandler.Evt.ShouldNotBeNull();
            myHandler.Evt.EventType.ShouldBe(ShortenerEventType.Get);
            myHandler.Evt.ShortUrl.ShouldNotBeNull();
            myHandler.Evt.ShortUrl.LongUrl.ShouldMatch(url);
            myHandler.Evt.ShortUrl.Id.ShouldMatch(shortUrl.Id);

            shortUrl.Id.ShouldMatch(lookup.Id);
            shortUrl.LongUrl.ShouldMatch(lookup.LongUrl);
        }
        [TestMethod]
        public async Task TestMethod_StoreUrl_RemoveUrl()
        {
            var url = "https://github.com/P7CoreOrg/dotnetcore.urlshortener/tree/dev";
            var store = _testServerFixture.GetService<IUrlShortenerStore>();
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
            var store = _testServerFixture.GetService<IUrlShortenerStore>();
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