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
            string connStr = _config.GetConnectionString("StudentHousingDb");

            // Als je nog geen DB wilt: voorkom crash als connStr leeg is
            if (string.IsNullOrWhiteSpace(connStr))
            {
                QuotesLijst.Add(("Database is nog niet gekoppeld.", ""));
                return;
            }

            using var conn = new MySqlConnection(connStr);
            conn.Open();

            string sql = @"SELECT tekst, auteur
                           FROM quotes
                           ORDER BY created_at DESC
                           LIMIT 5;";

            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            int tekstIndex = reader.GetOrdinal("tekst");
            int auteurIndex = reader.GetOrdinal("auteur");

            while (reader.Read())
            {
                string tekst = reader.GetString(tekstIndex);
                string auteur = reader.IsDBNull(auteurIndex) ? "" : reader.GetString(auteurIndex);
                QuotesLijst.Add((tekst, auteur));
            }
        }
    }
}
