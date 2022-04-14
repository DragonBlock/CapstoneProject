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
    public partial class Admin : System.Web.UI.Page
    {
        DatabaseAccessor accessor;

        protected void Page_Load(object sender, EventArgs e)
        {
            accessor = new DatabaseAccessor();
            accessor.ConnectToDatabase(Server.MapPath("/") + "FoodOrderingDB.mdb");

            gvOrders.DataSource = accessor.GetListOfOrders();
            gvOrders.DataBind();
            gvMenu.DataSource = accessor.GetItemList(ItemType.Main);
            gvMenu.DataBind();
            gvSides.DataSource = accessor.GetItemList(ItemType.Side);
            gvSides.DataBind();
            gvDrinks.DataSource = accessor.GetItemList(ItemType.Drink);
            gvDrinks.DataBind();
        }
    }
}