using System;

namespace dotnetcore.urlshortener.contracts
{
    public class ShortenerEventArgs : EventArgs
    {
        public ExpirationRedirectRecord ExpirationRedirectRecord { get; set; }
        public ShortUrl ShortUrl { get; set; }
        public ShortenerEventType EventType { get; set; }
        public DateTime UtcDateTime { get; set; }
    }
}