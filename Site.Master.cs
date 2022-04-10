using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CapstoneProject
{
    public partial class SiteMaster : MasterPage
    {
        public DatabaseAccessor accessor;

        protected void Page_Load(object sender, EventArgs e)
        {
            accessor = new DatabaseAccessor();
            accessor.ConnectToDatabase(Server.MapPath("/") + "FoodOrderingDB.mdb");
        }

        protected void btnAbout_Click(System.Object sender, System.EventArgs e)
        {
            Server.Transfer("About.aspx");
        }

        protected void btnHome_Click(object sender, EventArgs e)
        {
            Server.Transfer("Default.aspx");
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Server.Transfer("Login.aspx");
        }

        protected void Cntactbtn_Click(object sender, EventArgs e)
        {
            Server.Transfer("Contact.aspx");
        }
    }
}