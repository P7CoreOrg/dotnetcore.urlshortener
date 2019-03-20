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
    }
}