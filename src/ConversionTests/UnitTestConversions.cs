using System;
using System.Numerics;
using dotnetcore.urlshortener.generator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace ConversionTests
{
    [TestClass]
    public class UnitTestConversions
    {
        [TestMethod]
        public void TestMethod_Guid_to_BigInteger()
        {
            var s_guid = "{00d5db8e-4749-4bde-a5ce-5c98f290f102}";
            var guid = Guid.Parse(s_guid);
            var big = guid.ToBigInteger();
        //    var big = guid.ToBigInteger();
            var bigStr = big.ToString();
            bigStr.ShouldMatch("3912739421828898021718334793500187534");
        }
        [TestMethod]
        public void TestMethod_Encode_BigInteger()
        {
            var s_guid = "{00d5db8e-4749-4bde-a5ce-5c98f290f102}";
            var guid = Guid.Parse(s_guid);
            var big = guid.ToBigInteger();
            var encoded = ShortUrl.Encode(big);
            var decoded = ShortUrl.Decode(encoded);
            var bigStr = decoded.ToString();
            bigStr.ShouldMatch("3912739421828898021718334793500187534");

        }

        [TestMethod]
        public void TestMethod_Guid_to_Big_to_Guid()
        {
            var s_guid = "{00d5db8e-4749-4bde-a5ce-5c98f290f102}";
            var guid = Guid.Parse(s_guid);
            var big = guid.ToBigInteger();
            byte[] bytes = new byte[16];
            big.ToByteArray().CopyTo(bytes, 0);
            var guido = new Guid(bytes);

            guid.ToString().ShouldMatch(guido.ToString());

        }
    }

}
