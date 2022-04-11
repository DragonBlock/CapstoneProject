using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CapstoneProject
{
    public partial class SeeCart : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnMeals_Click(object sender, EventArgs e)
        {
            Response.Redirect("Menu-Meals.aspx");
        }

        protected void btnDrinks_Click(object sender, EventArgs e)
        {
            Response.Redirect("Menu-Drinks.aspx");
        }
    }
}