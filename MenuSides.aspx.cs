using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CapstoneProject
{
    public partial class MenuSides : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnMeals_Click(object sender, EventArgs e)
        {
            Server.Transfer("Menu-Meals.aspx");
        }

        protected void btnDrinks_Click(object sender, EventArgs e)
        {
            Server.Transfer("Menu-Drinks.aspx");
        }
    }
}