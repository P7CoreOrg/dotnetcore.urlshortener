using System;
using System.Linq;
using System.Numerics;

namespace dotnetcore.urlshortener.generator
{
    public static class GuidExtensions
    {
        public static BigInteger ToBigInteger(this Guid guid)
        {
            return new BigInteger(guid.ToByteArray());
        }

        public static string ToShortId(this Guid guid)
        {
            var big = guid.ToBigInteger();
            var encoded = ShortUrl.Encode(big);
            return encoded;
        }
        public static Guid FromShortId(this string shortId)
        {
            var decoded = ShortUrl.Decode(shortId);
            byte[] bytes = new byte[16];
            decoded.ToByteArray().CopyTo(bytes, 0);
            var guido = new Guid(bytes);
            return guido;
        }
    }
}