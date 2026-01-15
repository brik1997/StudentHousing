using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

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
            string? connStr = _config.GetConnectionString("StudentHousingDb");

            if (string.IsNullOrWhiteSpace(connStr))
            {
                QuotesLijst.Add(("Database is nog niet gekoppeld.", ""));
                return;
            }

            try
            {
                using var conn = new SqlConnection(connStr);
                conn.Open();

                // Optie A: datum = quote tekst
                // Let op: zonder ID/createdAt kunnen we "laatste" niet perfect bepalen.
                // Dit pakt gewoon 5 records.
                string sql = "SELECT TOP (5) datum, auteur FROM dbo.quotes;";

                using var cmd = new SqlCommand(sql, conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string tekst = reader.IsDBNull(0) ? "" : reader.GetString(0);   // datum -> tekst
                    string auteur = reader.IsDBNull(1) ? "" : reader.GetString(1);

                    QuotesLijst.Add((tekst, auteur));
                }

                if (QuotesLijst.Count == 0)
                {
                    QuotesLijst.Add(("Nog geen quotes gevonden.", ""));
                }
            }
            catch (Exception ex)
            {
                QuotesLijst.Add(("Database error:", ex.Message));
            }
        }
        

    }
}
