using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace HarvestHub
{
    public partial class Shop : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindProducts();
            }
        }

        private void BindProducts()
        {
            string connStr = ConfigurationManager.ConnectionStrings["HarvestHubDB"]?.ConnectionString;
            if (string.IsNullOrEmpty(connStr))
            {
                lblDebug.Text = "Error: Connection string 'HarvestHubDB' is missing in web.config.";
                lblDebug.Visible = true;
                return;
            }

            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            P.ProductID, 
                            P.ProductName, 
                            P.ProductDescription, 
                            P.ProductPrice, 
                            P.ProductImage,
                            ISNULL(SUM(C.Quantity), 0) AS CartQuantity
                        FROM Products P
                        LEFT JOIN Cart C ON P.ProductID = C.ProductID AND C.UserID = @UserID
                        GROUP BY P.ProductID, P.ProductName, P.ProductDescription, P.ProductPrice, P.ProductImage";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = Session["UserID"] != null ? Convert.ToInt32(Session["UserID"]) : 0;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }

                ProductsListView.DataSource = dt;
                ProductsListView.DataBind();
            }
            catch (Exception ex)
            {
                lblDebug.Text = "Error binding products: " + ex.Message;
                lblDebug.Visible = true;
                System.Diagnostics.Debug.WriteLine("BindProducts Error: " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        protected void ProductsListView_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            try
            {
                ListViewItem item = e.Item;
                int productId = Convert.ToInt32(e.CommandArgument);
                TextBox txtQuantity = (TextBox)item.FindControl("txtQuantity");
                int quantity = Convert.ToInt32(txtQuantity.Text);

                if (e.CommandName == "DecreaseQty")
                {
                    quantity = Math.Max(1, quantity - 1);
                    txtQuantity.Text = quantity.ToString();
                }
                else if (e.CommandName == "IncreaseQty")
                {
                    quantity = Math.Min(10, quantity + 1);
                    txtQuantity.Text = quantity.ToString();
                }
                else if (e.CommandName == "AddToCart")
                {
                    string result = AddToCart(productId, quantity);
                    if (result == "NotLoggedIn")
                    {
                        lblDebug.Text = "Please log in first.";
                        lblDebug.Visible = true;
                        Response.Redirect("Accounts.aspx");
                    }
                    else if (result == "Success")
                    {
                        lblDebug.Text = "Item added to cart!";
                        lblDebug.Visible = true;
                        BindProducts(); // Refresh ListView to update CartQuantity
                    }
                    else
                    {
                        lblDebug.Text = "Error: " + result;
                        lblDebug.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblDebug.Text = "Error in ItemCommand: " + ex.Message;
                lblDebug.Visible = true;
                System.Diagnostics.Debug.WriteLine("ItemCommand Error: " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private string AddToCart(int productId, int quantity)
        {
            if (Session["UserID"] == null)
                return "NotLoggedIn";

            if (!int.TryParse(Session["UserID"]?.ToString(), out int userId))
                return "Invalid UserID in session";

            if (productId <= 0 || quantity <= 0)
                return "Invalid product ID or quantity";

            string connStr = ConfigurationManager.ConnectionStrings["HarvestHubDB"]?.ConnectionString;
            if (string.IsNullOrEmpty(connStr))
                return "Connection string 'HarvestHubDB' is missing in web.config";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    // Verify ProductID exists
                    using (SqlCommand verifyProduct = new SqlCommand("SELECT COUNT(*) FROM Products WHERE ProductID = @ProductID", conn))
                    {
                        verifyProduct.Parameters.Add("@ProductID", SqlDbType.Int).Value = productId;
                        int productExists = Convert.ToInt32(verifyProduct.ExecuteScalar());
                        if (productExists == 0)
                            return "Product does not exist";
                    }

                    // Delete existing rows for this UserID and ProductID
                    using (SqlCommand delete = new SqlCommand("DELETE FROM Cart WHERE UserID = @UserID AND ProductID = @ProductID", conn))
                    {
                        delete.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
                        delete.Parameters.Add("@ProductID", SqlDbType.Int).Value = productId;
                        delete.ExecuteNonQuery();
                    }

                    // Insert new row with updated quantity
                    using (SqlCommand insert = new SqlCommand("INSERT INTO Cart (UserID, ProductID, Quantity) VALUES (@UserID, @ProductID, @Qty)", conn))
                    {
                        insert.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
                        insert.Parameters.Add("@ProductID", SqlDbType.Int).Value = productId;
                        insert.Parameters.Add("@Qty", SqlDbType.Int).Value = quantity;
                        insert.ExecuteNonQuery();
                    }
                }

                return "Success";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("AddToCart Error: " + ex.Message + "\n" + ex.StackTrace);
                return "Error: " + ex.Message;
            }
        }
    }
}