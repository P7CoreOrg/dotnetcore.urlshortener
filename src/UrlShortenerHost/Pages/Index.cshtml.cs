using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetcore.urlshortener.contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UrlShortenerHost.Pages
{
    public class IndexModel : PageModel
    {
        private IUrlShortenerService _urlShortenerService;

        public IndexModel(IUrlShortenerService urlShortenerService)
        {
            _urlShortenerService = urlShortenerService;
        }
        [BindProperty]
        public string LongUrl { get; set; }
        [BindProperty]
        public string ShortUrl { get; set; }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            bool isUri = Uri.IsWellFormedUriString(LongUrl, UriKind.Absolute);
            if (!isUri)
            {
                return Page();
            }

            var shortUrl = new ShortUrl
            {
                LongUrl = LongUrl,
                Exiration = DateTime.UtcNow.AddMinutes(2),
                ExpiredRedirectKey = "0001"
            };

            shortUrl = await _urlShortenerService.UpsertShortUrlAsync(shortUrl);
         
            this.ShortUrl = $"{Request.Scheme}://{Request.Host}/s/{shortUrl.Id}";
           
            return Page();
        }
    }
}
