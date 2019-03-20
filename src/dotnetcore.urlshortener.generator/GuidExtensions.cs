using System;
using System.Globalization;
using System.Linq;
using System.Numerics;

namespace dotnetcore.urlshortener.generator
{
    public static class GuidExtensions
    {
        public static string ToShortBase64(this Guid guid)
        {
            string enc = Convert.ToBase64String(guid.ToByteArray());
            enc = enc.Replace("/", "_");
            enc = enc.Replace("+", "-");
            return enc.Substring(0, 22);
        }
        public static Guid FromShortBase64(this string encoded)
        {
            encoded = encoded.Replace("_", "/");
            encoded = encoded.Replace("-", "+");
            byte[] buffer = Convert.FromBase64String(encoded + "==");
            return new Guid(buffer);
        }
    }
}