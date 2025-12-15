using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace eigen_website_1._0.Pages.KlachtenFormulier
{
    public class KlachtModel : PageModel
    {

        [BindProperty]
        public KlachtInput Formulier { get; set; }

        public class KlachtInput
        {
            public string Naam { get; set; }
            public string Gebouw { get; set; }
            public string Kamer { get; set; }
            public string Onderwerp { get; set; }
            public string Omschrijving { get; set; }
        }

        public void OnGet()
        {
        }

        
        public IActionResult OnPost()
        {
            string connectionString = "server=localhost;database=StudentHousing;user=root;password=Groep14;";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"INSERT INTO complaints
                (Naam, Gebouw, Kamer, Onderwerp, Omschrijving)
                VALUES (@Naam, @Gebouw, @Kamer, @Onderwerp, @Omschrijving)";


                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Naam", Formulier.Naam);
                cmd.Parameters.AddWithValue("@Gebouw", Formulier.Gebouw);
                cmd.Parameters.AddWithValue("@Kamer", Formulier.Kamer);
                cmd.Parameters.AddWithValue("@Onderwerp", Formulier.Onderwerp);
                cmd.Parameters.AddWithValue("@Omschrijving", Formulier.Omschrijving);


                cmd.ExecuteNonQuery();
            }

            return RedirectToPage("/KlachtenFormulier/Success");
        }
    }
}
