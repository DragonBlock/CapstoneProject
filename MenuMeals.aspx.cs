using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CapstoneProject
{
    public partial class MenuMeals : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSides_Click(object sender, EventArgs e)
        {
            Response.Redirect("Menu-Sides.aspx");
        }

        protected void btnDrinks_Click(object sender, EventArgs e)
        {
            Response.Redirect("Menu-Drinks.aspx");
        }
    }
}