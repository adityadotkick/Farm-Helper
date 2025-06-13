using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace HarvestHub
{
    public partial class Accounts : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        // User Login
        protected void btnUserLogin_Click(object sender, EventArgs e)
        {
            string username = txtUserLoginUsername.Text.Trim();
            string password = txtUserLoginPassword.Text.Trim();

            string connStr = ConfigurationManager.ConnectionStrings["HarvestHubDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT UserID FROM UsersTable WHERE Username=@Username AND Password=@Password", conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    Session["Username"] = username;
                    Session["UserID"] = Convert.ToInt32(result); // ✅ Needed for Cart
                    Response.Redirect("Shop.aspx");
                }
                else
                {
                    lblUserMessage.Text = "Invalid username or password.";
                }
            }
        }




        // User Registration
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string fullName = txtNewFullName.Text.Trim();
            string username = txtNewUsername.Text.Trim();
            string password = txtNewPassword.Text.Trim();

            string connStr = ConfigurationManager.ConnectionStrings["HarvestHubDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Check if username exists
                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM UsersTable WHERE Username=@Username", conn);
                checkCmd.Parameters.AddWithValue("@Username", username);

                int exists = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (exists > 0)
                {
                    lblRegisterMessage.Text = "Username already taken.";
                    lblRegisterMessage.CssClass = "text-danger d-block mt-3 text-center";
                }
                else
                {
                    // Insert New User
                    SqlCommand insertCmd = new SqlCommand("INSERT INTO UsersTable (FullName, Username, Password) VALUES (@FullName, @Username, @Password)", conn);
                    insertCmd.Parameters.AddWithValue("@FullName", fullName);
                    insertCmd.Parameters.AddWithValue("@Username", username);
                    insertCmd.Parameters.AddWithValue("@Password", password);

                    insertCmd.ExecuteNonQuery();

                    lblRegisterMessage.Text = "Registration successful! Please login.";
                    lblRegisterMessage.CssClass = "text-success d-block mt-3 text-center";
                }
            }
        }

        // Admin Login
        protected void btnAdminLogin_Click(object sender, EventArgs e)
        {
            string username = txtAdminUsername.Text.Trim();
            string password = txtAdminPassword.Text.Trim();

            string connStr = ConfigurationManager.ConnectionStrings["HarvestHubDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM AdminTable WHERE AdminUsername=@Username AND AdminPassword=@Password", conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                if (count == 1)
                {
                    // ✅ Create admin session
                    Session["AdminUsername"] = username;

                    lblAdminMessage.Text = "Admin login successful!";
                    lblAdminMessage.CssClass = "text-success d-block mt-3 text-center";

                    // Redirect to admin page
                    Response.Redirect("AdminDashboard.aspx");  // Create this later
                }

            }
        }

    }
}
