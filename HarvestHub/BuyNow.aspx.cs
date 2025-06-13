using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;
using System.Web;

namespace HarvestHub
{
    public partial class BuyNow : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProducts();
            }
        }

        private void LoadProducts()
        {
            string connStr = ConfigurationManager.ConnectionStrings["HarvestHubDB"].ConnectionString;
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Products", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            BuyNowListView.DataSource = dt;
            BuyNowListView.DataBind();
        }

        [WebMethod(EnableSession = true)]
        public static string AddToCart(int productId, int quantity)
        {
            if (HttpContext.Current.Session["UserID"] == null)
                return "NotLoggedIn";

            int userId = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
            string connStr = ConfigurationManager.ConnectionStrings["HarvestHubDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand check = new SqlCommand("SELECT COUNT(*) FROM Cart WHERE UserID=@UserID AND ProductID=@ProductID", conn);
                check.Parameters.AddWithValue("@UserID", userId);
                check.Parameters.AddWithValue("@ProductID", productId);

                int exists = Convert.ToInt32(check.ExecuteScalar());

                if (exists > 0)
                {
                    SqlCommand update = new SqlCommand("UPDATE Cart SET Quantity=@Qty WHERE UserID=@UserID AND ProductID=@ProductID", conn);
                    update.Parameters.AddWithValue("@Qty", quantity);
                    update.Parameters.AddWithValue("@UserID", userId);
                    update.Parameters.AddWithValue("@ProductID", productId);
                    update.ExecuteNonQuery();
                }
                else
                {
                    SqlCommand insert = new SqlCommand("INSERT INTO Cart (UserID, ProductID, Quantity) VALUES (@UserID, @ProductID, @Qty)", conn);
                    insert.Parameters.AddWithValue("@UserID", userId);
                    insert.Parameters.AddWithValue("@ProductID", productId);
                    insert.Parameters.AddWithValue("@Qty", quantity);
                    insert.ExecuteNonQuery();
                }
            }

            return "Success";
        }
    }
}
