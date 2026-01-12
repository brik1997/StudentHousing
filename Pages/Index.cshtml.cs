using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace eigen_website_1._0.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _config;

        public List<(string Tekst, string Auteur)> QuotesLijst { get; set; } = new();

        public IndexModel(ILogger<IndexModel> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public void OnGet()
        {
            // Tijdelijk: database uitgeschakeld
            QuotesLijst.Add(("Quotes komen later.", "Database is nog niet actief"));
        }

    }
}
