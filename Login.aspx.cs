using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CapstoneProject
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        DatabaseAccessor accessor;

        protected void Page_Load(object sender, EventArgs e)
        {
            //accessor.ConnectToDatabase();
        }

        protected void LoginToWebsite_Authenticate(object sender, AuthenticateEventArgs e)
        {

            
        }
    }
}