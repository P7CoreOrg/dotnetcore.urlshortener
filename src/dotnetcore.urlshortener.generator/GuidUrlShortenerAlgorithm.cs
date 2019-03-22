using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using dotnetcore.urlshortener.contracts;
using dotnetcore.urlshortener.generator.Extensions;

namespace dotnetcore.urlshortener.generator
{
    public class GuidUrlShortenerAlgorithm : IUrlShortenerAlgorithm
    {
        public Task<string> GenerateUniqueId()
        {
            var guid = Guid.NewGuid();
            var id = guid.ToShortBase64();
            return Task.FromResult(id);
        }
    }
}
