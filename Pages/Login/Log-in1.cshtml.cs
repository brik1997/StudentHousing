using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace eigen_website_1._0.Pages.Log_in
{
    public class Log_in1Model : PageModel
    {
        
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public IActionResult OnPost()
        {
            string connStr = "server=localhost;database=studenthousing;user=root;password=Groep14;";

            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string sql = "SELECT * FROM users WHERE Email = @email AND PasswordHash = @pass";

                var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@email", Email);
                cmd.Parameters.AddWithValue("@pass", Password);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // SESSIONS
                        HttpContext.Session.SetInt32("UserId", reader.GetInt32("Id"));
                        HttpContext.Session.SetString("Role", reader.GetString("Role"));
                        HttpContext.Session.SetString("Email", reader.GetString("Email"));

                        // Redirect op basis van rol
                        string role = reader.GetString("Role");

                        if (role == "Admin")
                            return RedirectToPage("/AdminDashboard/Index");

                        if (role == "Student")
                            return RedirectToPage("/Dashboard/Index");
                    }
                    else
                    {
                        ErrorMessage = "Foutieve login!";
                        return Page();
                    }
                }
            }

            return Page();
        }
    
        public void OnGet()
        {
        }
    }
}
