using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace HarvestHub
{
    public partial class TrackCrop : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Username"] == null)
                {
                    Response.Redirect("Accounts.aspx");
                    return;
                }
                LoadAvailableCrops();
            }
        }

        private void LoadAvailableCrops()
        {
            string connStr = ConfigurationManager.ConnectionStrings["HarvestHubDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT CropName FROM CropMaster", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                ddlAvailableCrops.Items.Clear();
                ddlAvailableCrops.Items.Add(new ListItem("Select Crop", "0"));
                while (reader.Read())
                {
                    ddlAvailableCrops.Items.Add(new ListItem(reader["CropName"].ToString(), reader["CropName"].ToString()));
                }
                conn.Close();
            }
        }

        protected void ddlAvailableCrops_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAvailableYields();
        }

        private void LoadAvailableYields()
        {
            ddlYield.Items.Clear();
            ddlYield.Items.Add(new ListItem("New Yield", "0"));

            string username = Session["Username"].ToString();
            int userId = GetUserIdFromUsername(username);
            int cropId = GetCropIdByName(ddlAvailableCrops.SelectedValue);

            string connStr = ConfigurationManager.ConnectionStrings["HarvestHubDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT DISTINCT YieldNumber FROM UserCropTracking WHERE UserID = @UserID AND CropID = @CropID", conn);
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@CropID", cropId);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int yieldNum = Convert.ToInt32(reader["YieldNumber"]);
                    ddlYield.Items.Add(new ListItem($"Yield {yieldNum}", yieldNum.ToString()));
                }
                conn.Close();
            }
        }

        protected void ddlYield_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadUserCropActivities();
        }

        protected void btnStartTracking_Click(object sender, EventArgs e)
        {
            if (ddlAvailableCrops.SelectedValue == "0")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please select a crop.');", true);
                return;
            }

            try
            {
                string selectedCrop = ddlAvailableCrops.SelectedValue;
                string username = Session["Username"].ToString();
                int userId = GetUserIdFromUsername(username);
                int cropId = GetCropIdByName(selectedCrop);
                int yieldNumber = ddlYield.SelectedValue == "0" ? GetNextYieldNumber(userId, cropId) : Convert.ToInt32(ddlYield.SelectedValue);

                string connStr = ConfigurationManager.ConnectionStrings["HarvestHubDB"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand insertTracking = new SqlCommand("INSERT INTO UserCropTracking (UserID, CropID, YieldNumber, StartDate) OUTPUT INSERTED.TrackingID VALUES (@UserID, @CropID, @YieldNumber, GETDATE())", conn);
                    insertTracking.Parameters.AddWithValue("@UserID", userId);
                    insertTracking.Parameters.AddWithValue("@CropID", cropId);
                    insertTracking.Parameters.AddWithValue("@YieldNumber", yieldNumber);
                    int trackingId = (int)insertTracking.ExecuteScalar();

                    string[] steps = { "Planting", "Watering", "Pesticide", "Harvest" };
                    foreach (string step in steps)
                    {
                        SqlCommand insertStep = new SqlCommand("INSERT INTO UserCropActivities (TrackingID, StepName, Status, TemperatureInfo, Notes, DueDate) VALUES (@TrackingID, @StepName, 'Pending', @Temp, @Notes, @DueDate)", conn);
                        insertStep.Parameters.AddWithValue("@TrackingID", trackingId);
                        insertStep.Parameters.AddWithValue("@StepName", step);
                        insertStep.Parameters.AddWithValue("@Temp", step == "Planting" ? "15°C - 20°C" : "Maintain optimal conditions");
                        insertStep.Parameters.AddWithValue("@Notes", step == "Pesticide" ? "Use organic pesticide." : "Follow crop-specific guidance.");
                        insertStep.Parameters.AddWithValue("@DueDate", DateTime.Now.AddDays(3));
                        insertStep.ExecuteNonQuery();
                    }
                    conn.Close();
                }
                ClientScript.RegisterStartupScript(this.GetType(), "success", "alert('Tracking started successfully!'); window.location='TrackCrop.aspx';", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "error", $"alert('Error: {ex.Message}');", true);
            }
        }

        private int GetNextYieldNumber(int userId, int cropId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["HarvestHubDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT ISNULL(MAX(YieldNumber), 0) + 1 FROM UserCropTracking WHERE UserID = @UserID AND CropID = @CropID", conn);
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@CropID", cropId);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void LoadUserCropActivities()
        {
            string username = Session["Username"].ToString();
            int userId = GetUserIdFromUsername(username);
            int cropId = GetCropIdByName(ddlAvailableCrops.SelectedValue);
            int yieldNumber = Convert.ToInt32(ddlYield.SelectedValue);

            string connStr = ConfigurationManager.ConnectionStrings["HarvestHubDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand trackingCmd = new SqlCommand("SELECT TrackingID FROM UserCropTracking WHERE UserID = @UserID AND CropID = @CropID AND YieldNumber = @Yield", conn);
                trackingCmd.Parameters.AddWithValue("@UserID", userId);
                trackingCmd.Parameters.AddWithValue("@CropID", cropId);
                trackingCmd.Parameters.AddWithValue("@Yield", yieldNumber);

                object trackingResult = trackingCmd.ExecuteScalar();
                if (trackingResult == null)
                {
                    rptCropActivities.DataSource = null;
                    rptCropActivities.DataBind();
                    return;
                }

                int trackingId = Convert.ToInt32(trackingResult);
                SqlCommand actCmd = new SqlCommand("SELECT * FROM UserCropActivities WHERE TrackingID = @TrackingID ORDER BY ActivityID", conn);
                actCmd.Parameters.AddWithValue("@TrackingID", trackingId);

                SqlDataAdapter da = new SqlDataAdapter(actCmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptCropActivities.DataSource = dt;
                rptCropActivities.DataBind();
                conn.Close();
            }
        }

        protected void rptCropActivities_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "MarkCompleted")
            {
                int activityId = Convert.ToInt32(e.CommandArgument);
                string connStr = ConfigurationManager.ConnectionStrings["HarvestHubDB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE UserCropActivities SET Status = 'Completed', CompletedDate = GETDATE() WHERE ActivityID = @ActivityID", conn);
                    cmd.Parameters.AddWithValue("@ActivityID", activityId);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                LoadUserCropActivities();
            }
        }

        private int GetUserIdFromUsername(string username)
        {
            string connStr = ConfigurationManager.ConnectionStrings["HarvestHubDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT UserID FROM UsersTable WHERE Username = @Username", conn);
                cmd.Parameters.AddWithValue("@Username", username);
                object result = cmd.ExecuteScalar();
                conn.Close();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        private int GetCropIdByName(string cropName)
        {
            string connStr = ConfigurationManager.ConnectionStrings["HarvestHubDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT CropID FROM CropMaster WHERE CropName = @CropName", conn);
                cmd.Parameters.AddWithValue("@CropName", cropName);
                object result = cmd.ExecuteScalar();
                conn.Close();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }
    }
}
