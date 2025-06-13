using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

namespace HarvestHub
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Username"] != null) // Logged in as User
                {
                    phUserLinks.Visible = true;
                    phAdminLinks.Visible = false;
                    phUserLoggedIn.Visible = true;
                    phAccountLink.Visible = false;
                    phCartIcon.Visible = true; // ✅ Show cart for user
                    lblWelcomeUser.Text = "Welcome, " + Session["Username"].ToString();
                }
                else if (Session["AdminUsername"] != null) // Logged in as Admin
                {
                    phUserLinks.Visible = false;
                    phAdminLinks.Visible = true;
                    phUserLoggedIn.Visible = true;
                    phAccountLink.Visible = false;
                    phCartIcon.Visible = false; // ❌ Hide cart for admin
                    lblWelcomeUser.Text = "Admin Panel";
                }
                else // Not logged in
                {
                    phUserLinks.Visible = true;
                    phAdminLinks.Visible = false;
                    phUserLoggedIn.Visible = false;
                    phAccountLink.Visible = true;
                    phCartIcon.Visible = true; // ✅ Show cart for public users
                }
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Accounts.aspx");
        }

        // ✅ WebMethod to return cart item count dynamically
        [WebMethod(EnableSession = true)]
        public static int GetCartItemCount()
        {
            if (HttpContext.Current.Session["UserID"] == null)
                return 0;

            int userId = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["HarvestHubDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT ISNULL(SUM(Quantity), 0) FROM Cart WHERE UserID = @UserID", conn);
                cmd.Parameters.AddWithValue("@UserID", userId);
                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }
    }
}
