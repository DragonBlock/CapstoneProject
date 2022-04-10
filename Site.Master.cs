using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CapstoneProject
{
    public partial class SiteMaster : MasterPage
    {
        public DatabaseAccessor accessor;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies.Count == 0)
            {
                btnAdmin0.Visible = false;
                btnAdmin0.Enabled = false;
                btnAccount0.Visible = false;
                btnAccount0.Enabled = false;
                btnLogin0.Visible = true;
                btnLogin0.Enabled = true;
                btnSignUp0.Visible = true;
                btnSignUp0.Enabled = true;
                btnLogOut.Visible = false;
                btnLogOut.Enabled = false;
                
            }
            else if (Request.Cookies["LoginInfo"].Value.Contains("Customer"))
            {
                btnAdmin0.Visible = false;
                btnAdmin0.Enabled = false;
                btnAccount0.Visible = true;
                btnAccount0.Enabled = true;
                btnLogin0.Visible = false;
                btnLogin0.Enabled = false;
                btnSignUp0.Visible = false;
                btnSignUp0.Enabled = false;
                btnLogOut.Visible = true;
                btnLogOut.Enabled = true;
            }
            else if (Request.Cookies["LoginInfo"].Value.Contains("Employee"))
            {
                btnAdmin0.Visible = true;
                btnAdmin0.Enabled = true;
                btnAccount0.Visible = false;
                btnAccount0.Enabled = false;
                btnLogin0.Visible = false;
                btnLogin0.Enabled = false;
                btnSignUp0.Visible = false;
                btnSignUp0.Enabled = false;
                btnLogOut.Visible = true;
                btnLogOut.Enabled = true;
            }
            else
            {
                btnAdmin0.Visible = false;
                btnAdmin0.Enabled = false;
                btnAccount0.Visible = false;
                btnAccount0.Enabled = false;
                btnLogin0.Visible = true;
                btnLogin0.Enabled = true;
                btnSignUp0.Visible = true;
                btnSignUp0.Enabled = true;
                btnLogOut.Visible = false;
                btnLogOut.Enabled = false;
            }
            accessor = new DatabaseAccessor();
            accessor.ConnectToDatabase(Server.MapPath("/") + "FoodOrderingDB.mdb");
        }

        protected void btnAbout_Click(System.Object sender, System.EventArgs e)
        {
            Server.Transfer("About.aspx");
        }


        protected void btnMenu_Click(System.Object sender, System.EventArgs e)
        {
            Server.Transfer("Menu-Meals.aspx");
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

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            Request.Cookies.Clear();
            Server.Transfer("Login.aspx");
        }

        protected void btnAccount0_Click(object sender, EventArgs e)
        {
            Server.Transfer("ViewAccount.aspx");
        }
    }
}