using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HarvestHub
{
    public partial class Cart : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCart();
            }
        }

        private void LoadCart()
        {
            DataTable cartTable = GetCartData();

            if (cartTable != null && cartTable.Rows.Count > 0)
            {
                CartRepeater.DataSource = cartTable;
                CartRepeater.DataBind();

                decimal total = 0;
                foreach (DataRow row in cartTable.Rows)
                {
                    total += Convert.ToDecimal(row["Subtotal"]);
                }
                lblTotal.Text = "₹" + total.ToString("F2");

                CartTable.Visible = true;
                lblEmpty.Visible = false;
            }
            else
            {
                CartTable.Visible = false;
                lblEmpty.Visible = true;
                lblEmpty.Text = "No items in your cart.";
                lblEmpty.CssClass = "text-muted text-center d-block";
            }
        }

        private DataTable GetCartData()
        {
            if (Session["UserID"] == null)
            {
                lblDebug.Text = "Please log in to view your cart.";
                lblDebug.Visible = true;
                return null;
            }

            if (!int.TryParse(Session["UserID"]?.ToString(), out int userId))
            {
                lblDebug.Text = "Invalid UserID in session.";
                lblDebug.Visible = true;
                return null;
            }

            string connStr = ConfigurationManager.ConnectionStrings["HarvestHubDB"]?.ConnectionString;
            if (string.IsNullOrEmpty(connStr))
            {
                lblDebug.Text = "Error: Connection string 'HarvestHubDB' is missing in web.config.";
                lblDebug.Visible = true;
                return null;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            P.ProductName,
                            P.ProductPrice,
                            SUM(C.Quantity) AS Quantity,
                            (P.ProductPrice * SUM(C.Quantity)) AS Subtotal,
                            C.ProductID
                        FROM Cart C
                        INNER JOIN Products P ON C.ProductID = P.ProductID
                        WHERE C.UserID = @UserID
                        GROUP BY P.ProductName, P.ProductPrice, C.ProductID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    return dt;
                }
            }
            catch (Exception ex)
            {
                lblDebug.Text = "Error loading cart: " + ex.Message;
                lblDebug.Visible = true;
                System.Diagnostics.Debug.WriteLine("GetCartData Error: " + ex.Message + "\n" + ex.StackTrace);
                return null;
            }
        }

        protected void CartRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (Session["UserID"] == null)
                {
                    lblDebug.Text = "Please log in to update your cart.";
                    lblDebug.Visible = true;
                    Response.Redirect("Accounts.aspx");
                    return;
                }

                if (!int.TryParse(Session["UserID"]?.ToString(), out int userId))
                {
                    lblDebug.Text = "Invalid UserID in session.";
                    lblDebug.Visible = true;
                    return;
                }

                int productId = Convert.ToInt32(e.CommandArgument);
                TextBox txtQuantity = (TextBox)e.Item.FindControl("txtQuantity");
                int currentQuantity = Convert.ToInt32(txtQuantity.Text);

                int newQuantity = currentQuantity;
                if (e.CommandName == "IncreaseQty")
                {
                    newQuantity = Math.Min(10, currentQuantity + 1); // Max 10
                }
                else if (e.CommandName == "DecreaseQty")
                {
                    newQuantity = Math.Max(1, currentQuantity - 1); // Min 1
                }

                if (newQuantity != currentQuantity)
                {
                    string result = UpdateCartQuantity(userId, productId, newQuantity);
                    if (result == "Success")
                    {
                        LoadCart(); // Refresh cart display
                    }
                    else
                    {
                        lblDebug.Text = "Error updating quantity: " + result;
                        lblDebug.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblDebug.Text = "Error updating cart: " + ex.Message;
                lblDebug.Visible = true;
                System.Diagnostics.Debug.WriteLine("CartRepeater_ItemCommand Error: " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private string UpdateCartQuantity(int userId, int productId, int quantity)
        {
            string connStr = ConfigurationManager.ConnectionStrings["HarvestHubDB"]?.ConnectionString;
            if (string.IsNullOrEmpty(connStr))
                return "Connection string 'HarvestHubDB' is missing in web.config";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    // Verify ProductID exists
                    SqlCommand verifyProduct = new SqlCommand("SELECT COUNT(*) FROM Products WHERE ProductID = @ProductID", conn);
                    verifyProduct.Parameters.AddWithValue("@ProductID", productId);
                    int productExists = Convert.ToInt32(verifyProduct.ExecuteScalar());
                    if (productExists == 0)
                        return "Product does not exist";

                    // Delete existing rows for this UserID and ProductID
                    SqlCommand delete = new SqlCommand("DELETE FROM Cart WHERE UserID = @UserID AND ProductID = @ProductID", conn);
                    delete.Parameters.AddWithValue("@UserID", userId);
                    delete.Parameters.AddWithValue("@ProductID", productId);
                    delete.ExecuteNonQuery();

                    // Insert new row with updated quantity
                    SqlCommand insert = new SqlCommand("INSERT INTO Cart (UserID, ProductID, Quantity) VALUES (@UserID, @ProductID, @Qty)", conn);
                    insert.Parameters.AddWithValue("@UserID", userId);
                    insert.Parameters.AddWithValue("@ProductID", productId);
                    insert.Parameters.AddWithValue("@Qty", quantity);
                    insert.ExecuteNonQuery();

                    return "Success";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("UpdateCartQuantity Error: " + ex.Message + "\n" + ex.StackTrace);
                return "Error: " + ex.Message;
            }
        }

        protected void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            lblDebug.Visible = false; // Reset debug label

            // Check if user is logged in
            if (Session["UserID"] == null)
            {
                lblDebug.Text = "Please log in to place an order.";
                lblDebug.Visible = true;
                Response.Redirect("Accounts.aspx");
                return;
            }

            if (!int.TryParse(Session["UserID"]?.ToString(), out int userId))
            {
                lblDebug.Text = "Invalid UserID in session.";
                lblDebug.Visible = true;
                return;
            }

            // Check if cart is empty
            DataTable cartTable = GetCartData();
            if (cartTable == null || cartTable.Rows.Count == 0)
            {
                lblDebug.Text = "Cannot place order: Your cart is empty.";
                lblDebug.Visible = true;
                return;
            }

            // Validate form inputs
            if (!Page.IsValid)
            {
                lblDebug.Text = "Please correct the form errors above.";
                lblDebug.Visible = true;
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["HarvestHubDB"]?.ConnectionString;
            if (string.IsNullOrEmpty(connStr))
            {
                lblDebug.Text = "Error: Connection string 'HarvestHubDB' is missing in web.config.";
                lblDebug.Visible = true;
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Cart WHERE UserID = @UserID", conn);
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.ExecuteNonQuery();
                }

                // Clear form and show success message
                txtName.Text = "";
                txtPhone.Text = "";
                txtAddress.Text = "";

                CartTable.Visible = false;
                lblEmpty.Text = "✅ Order placed successfully!";
                lblEmpty.CssClass = "text-success text-center d-block";
                lblEmpty.Visible = true;
            }
            catch (Exception ex)
            {
                lblDebug.Text = "Error placing order: " + ex.Message;
                lblDebug.Visible = true;
                System.Diagnostics.Debug.WriteLine("btnPlaceOrder_Click Error: " + ex.Message + "\n" + ex.StackTrace);
            }
        }
    }
}