using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CapstoneProject
{
    public partial class MenuDrinks : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnMeals_Click(object sender, EventArgs e)
        {
            Response.Redirect("MenuMeals.aspx");
        }

        protected void btnSides_Click(object sender, EventArgs e)
        {
            Response.Redirect("MenuSides.aspx");
        }
    }
}