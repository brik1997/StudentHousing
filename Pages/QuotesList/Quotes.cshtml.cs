using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace eigen_website_1._0.Pages.QuotesList
{
    public class QuotesModel : PageModel
    {
        [BindProperty]
        public string Tekst { get; set; }

        [BindProperty]
        public string Auteur { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            string connStr = "server=localhost;database=studenthousing;user=root;password=Groep14;";

            using var conn = new MySqlConnection(connStr);
            conn.Open();

            string sql = @"INSERT INTO quotes (tekst, auteur)
                           VALUES (@tekst, @auteur);";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@tekst", Tekst);
            cmd.Parameters.AddWithValue("@auteur", Auteur);

            cmd.ExecuteNonQuery();

            return RedirectToPage("/Index");
        }
    }
}
