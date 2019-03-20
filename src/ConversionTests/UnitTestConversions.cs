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
        public void TestMethod_Guid_to_ShortBase64_to_Guid()
        {
           
            var s_guid = "{00d5db8e-4749-4bde-a5ce-5c98f290f102}";
            var guid = Guid.Parse(s_guid);
            var encoded = guid.ToShortBase64();
            encoded.ShouldNotBeNullOrEmpty();
            var decoded = encoded.FromShortBase64();
            decoded.ShouldNotBeNull();
            guid.ToString().ShouldMatch(decoded.ToString());
        }
    }

}
