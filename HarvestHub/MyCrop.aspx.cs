using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HarvestHub
{
    public partial class MyCrop : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
                // ✅ Check if user or admin is logged in
                //if (Session["Username"] == null && Session["AdminUsername"] == null)
                //{
                    //Response.Redirect("Accounts.aspx");
                //}
            //}
        }

        protected void btnTrackSaffron_Click(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                // User not logged in, redirect to login
                Response.Redirect("Accounts.aspx");
            }
            else
            {
                // User logged in, redirect to crop tracking page
                Response.Redirect("TrackCrop.aspx");
            }
        }

        protected void btnTrackMushroom_Click(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                // User not logged in, redirect to login
                Response.Redirect("Accounts.aspx");
            }
            else
            {
                // User logged in, redirect to crop tracking page
                Response.Redirect("TrackCrop.aspx");
            }
        }

    }
}