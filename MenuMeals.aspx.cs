using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CapstoneProject
{
    public partial class MenuMeals : Page
    {
        DatabaseAccessor accessor;
        protected void Page_Load(object sender, EventArgs e)
        {
            accessor = new DatabaseAccessor();
            accessor.ConnectToDatabase(Server.MapPath("/") + "FoodOrderingDB.mdb");

            List<Panel> panelsList = new List<Panel>();
            DataTable foodTable = accessor.GetItemList(ItemType.Main);
            foreach (DataRow foodRow in foodTable.Rows)
            {
                Panel p = new Panel();
                p.ID = "pan" + foodRow.Field<int>("Item_ID");
                p.CssClass = "MenuItem";

                Image i = new Image();
                i.ID = "img" + foodRow.Field<int>("Item_ID");
                i.ImageUrl = Server.MapPath("/Images/") + foodRow.Field<string>("Item_Image");
                i.ImageAlign = ImageAlign.Middle;
                i.CssClass = "MenuImage";
                Label l = new Label();
                l.ID = "lbl" + foodRow.Field<int>("Item_ID");
                l.CssClass = "MenuLabel";
                l.Text = foodRow.Field<string>("Item_Name") + " $" + foodRow.Field<decimal>("Item_Price");

                Button b = new Button();
                b.Click += Item_Click;
                b.ID = "btn" + foodRow.Field<int>("Item_ID");
                b.CssClass = "MenuButton";
                b.Text = "Add To Order";
                

                p.Controls.Add(i);
                p.Controls.Add(l);
                p.Controls.Add(b);

                lock (Controls.SyncRoot)
                {
                    MainPanel.Controls.Add(p);
                }

                panelsList.Add(p);
            }

        }

        protected void Item_Click(object sender, EventArgs e)
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