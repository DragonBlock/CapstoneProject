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
    public partial class Login : System.Web.UI.Page
    {
        DatabaseAccessor accessor;

        protected void Page_Load(object sender, EventArgs e)
        {
            accessor = new DatabaseAccessor();
            accessor.ConnectToDatabase(Server.MapPath("/")+ "FoodOrderingDB.mdb");
        }

        protected void LoginToWebsite_Authenticate(object sender, AuthenticateEventArgs e)
        {
            KeyValuePair<AccountType, int> userID = 
                accessor.ValidateLogin(LoginToWebsite.UserName, LoginToWebsite.Password);
            if(userID.Value == -1)
            {
                LoginToWebsite.InstructionText = "Invalid username/password.";
                LoginToWebsite.InstructionTextStyle.ForeColor = System.Drawing.Color.RosyBrown;
                e.Authenticated = false;
            }
            else
            { 
                e.Authenticated = true;
                if (userID.Key == AccountType.Customer)
                {

                    HttpCookie cookie = new HttpCookie("LoginInfo", "Customer" + userID.Value);
                    Response.AppendCookie(cookie);

                    LoginToWebsite.DestinationPageUrl = "ViewAccount.aspx";
                }
                else
                {
                    HttpCookie cookie = new HttpCookie("LoginInfo", "Employee" + userID.Value);
                    Response.AppendCookie(cookie);

                    LoginToWebsite.DestinationPageUrl = "Admin.aspx";
                }
            } 
        }
    }
}