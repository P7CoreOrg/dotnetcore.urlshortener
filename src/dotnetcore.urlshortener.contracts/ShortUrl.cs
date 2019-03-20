using System;

namespace dotnetcore.urlshortener.contracts
{
    public class ShortUrl
    {
        public string LongUrl { get; set; }
        public string Id { get; set; }
        public DateTime Exiration { get; set; }
    }
}