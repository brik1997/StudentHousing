using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace eigen_website_1._0.Pages.QuotesList
{
    public class QuotesModel : PageModel
    {
        private readonly IConfiguration _config;

        public QuotesModel(IConfiguration config)
        {
            _config = config;
        }

        [BindProperty]
        public string Tekst { get; set; } = "";

        [BindProperty]
        public string Auteur { get; set; } = "";

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(Tekst))
            {
                ModelState.AddModelError("", "Quote tekst is verplicht.");
                return Page();
            }

            string? connStr = _config.GetConnectionString("StudentHousingDb");
            if (string.IsNullOrWhiteSpace(connStr))
            {
                ModelState.AddModelError("", "Database connectie ontbreekt.");
                return Page();
            }

            using var conn = new SqlConnection(connStr);
            conn.Open();

            // Optie A: we misbruiken 'datum' als quote-tekst
            string sql = @"INSERT INTO dbo.quotes (auteur, datum)
                           VALUES (@auteur, @datum);";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@auteur", string.IsNullOrWhiteSpace(Auteur) ? "" : Auteur);
            cmd.Parameters.AddWithValue("@datum", Tekst);

            cmd.ExecuteNonQuery();

            return RedirectToPage("/Index");
        }
    }
}
