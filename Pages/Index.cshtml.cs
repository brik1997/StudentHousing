using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;



namespace eigen_website_1._0.Pages
{

    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public List<(string Tekst, string Auteur)> QuotesLijst { get; set; } = new();


        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            string connStr = "server=localhost;database=studenthousing;user=root;password=Groep14;";

            using var conn = new MySqlConnection(connStr);
            conn.Open();

            
            string sql = @"SELECT tekst, auteur 
                   FROM quotes 
                   ORDER BY created_at DESC 
                   LIMIT 5;";

            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string tekst = reader.GetString("tekst");
                string auteur = reader.IsDBNull("auteur") ? "" : reader.GetString("auteur");
                QuotesLijst.Add((tekst, auteur));
            }
        }

    }
}
