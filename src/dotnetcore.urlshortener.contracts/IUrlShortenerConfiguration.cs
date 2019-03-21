using System.Threading.Tasks;

namespace dotnetcore.urlshortener.contracts
{
    public interface IUrlShortenerConfiguration
    {
        Task<ExpirationRedirectRecord> GetExpirationRedirectRecordAsync(string key);
    }
}