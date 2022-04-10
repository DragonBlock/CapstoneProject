using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CapstoneProject
{
    public partial class SignUp : System.Web.UI.Page
    {
        DatabaseAccessor accessor;

        protected void Page_Load(object sender, EventArgs e)
        {
            accessor = new DatabaseAccessor();
            accessor.ConnectToDatabase(Server.MapPath("/")+ "FoodOrderingDB.mdb");
        }

        protected void btnSignUp_Click(object sender, EventArgs e)
        {
            if (tbUser.Text.ToString().Contains(" "))
            {
                lblMessage.Text = "Error: Username cannot contain spaces.";
            }
            else if (!accessor.ValidateNewAccount(tbUser.Text.ToString()))
            {
                lblMessage.Text = "Error: Username already taken.";
            }
            else if (tbPass.Text.ToString().Contains(" "))
            {
                lblMessage.Text = "Error: Password cannot contain spaces.";
            }
            else if (!tbEmail.Text.ToString().Contains("@") || !tbEmail.Text.ToString().Contains("."))
            {
                lblMessage.Text = "Error: Invalid email.";
            }
            else if (tbUser.Text.ToString() == "") lblMessage.Text = "Error: Please enter a username.";
            else if (tbPass.Text.ToString() == "") lblMessage.Text = "Error: Please enter a password.";
            else if (tbEmail.Text.ToString() == "") lblMessage.Text = "Error: Please enter a email.";
            else if (tbFName.Text.ToString() == "") lblMessage.Text = "Error: Please enter a first name.";
            else if (tbLName.Text.ToString() == "") lblMessage.Text = "Error: Please enter a last name.";
            else
            {
                bool worked = accessor.CreateAccount(tbUser.Text.ToString(), tbPass.Text.ToString(), 
                    tbFName.Text.ToString(), tbLName.Text.ToString(), 
                    tbEmail.Text.ToString(), tbPhoneNum.Text.ToString());
                if (worked)
                {
                    lblMessage.Text = "Account Created";
                    Response.Redirect("Login.aspx");
                }
                else lblMessage.Text = "Error: Invalid information.";
            }
        }
    }
}