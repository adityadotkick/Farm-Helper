using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace HarvestHub
{
    public partial class AdminDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["AdminUsername"] == null)
                {
                    Response.Redirect("Accounts.aspx");
                }
                LoadProducts();
            }
        }

        private void LoadProducts()
        {
            string connStr = ConfigurationManager.ConnectionStrings["HarvestHubDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Products", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvProducts.DataSource = dt;
                gvProducts.DataBind();
            }
        }

        protected void btnSaveNewProduct_Click(object sender, EventArgs e)
        {
            string name = txtAddName.Text.Trim();
            string desc = txtAddDescription.Text.Trim();
            decimal price = Convert.ToDecimal(txtAddPrice.Text.Trim());
            string imageFileName = "";

            if (fuAddImage.HasFile)
            {
                string ext = Path.GetExtension(fuAddImage.FileName).ToLower();
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

                // ✅ Use extItem instead of e to avoid CS0136
                if (Array.Exists(allowedExtensions, extItem => extItem == ext))
                {
                    imageFileName = Guid.NewGuid().ToString() + ext;
                    string savePath = Server.MapPath("~/images/") + imageFileName;
                    fuAddImage.SaveAs(savePath);
                }
                else
                {
                    lblImageMessage.Text = "Only JPG, PNG, and GIF images are allowed.";
                    lblImageMessage.CssClass = "text-danger";
                    lblImageMessage.Visible = true;
                    return;
                }
            }
            else
            {
                lblImageMessage.Text = "Please upload an image file.";
                lblImageMessage.CssClass = "text-danger";
                lblImageMessage.Visible = true;
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["HarvestHubDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Products (ProductName, ProductDescription, ProductPrice, ProductImage) VALUES (@Name, @Desc, @Price, @Image)", conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Desc", desc);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@Image", imageFileName);
                cmd.ExecuteNonQuery();
            }

            LoadProducts();
        }


        protected void btnUpdateProduct_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(hfEditProductID.Value);
            string name = txtEditName.Text.Trim();
            string desc = txtEditDescription.Text.Trim();
            decimal price = Convert.ToDecimal(txtEditPrice.Text.Trim());
            string image = txtEditImage.Text.Trim();

            string connStr = ConfigurationManager.ConnectionStrings["HarvestHubDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Products SET ProductName=@Name, ProductDescription=@Desc, ProductPrice=@Price, ProductImage=@Image WHERE ProductID=@ID", conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Desc", desc);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@Image", image);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.ExecuteNonQuery();
            }

            LoadProducts();
        }

        protected void gvProducts_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteProduct")
            {
                int id = Convert.ToInt32(e.CommandArgument);

                string connStr = ConfigurationManager.ConnectionStrings["HarvestHubDB"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Products WHERE ProductID=@ID", conn);
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.ExecuteNonQuery();
                }

                LoadProducts();
            }
        }
    }
}
