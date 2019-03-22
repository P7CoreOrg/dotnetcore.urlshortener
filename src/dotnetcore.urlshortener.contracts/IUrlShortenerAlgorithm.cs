using System.Threading.Tasks;

namespace dotnetcore.urlshortener.contracts
{
    public interface IUrlShortenerAlgorithm
    {
        Task<string> GenerateUniqueId();
    }
}