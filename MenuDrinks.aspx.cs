using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace CapstoneProject
{
    public partial class MenuDrinks : Page
    {
        DatabaseAccessor accessor;
        protected void Page_Load(object sender, EventArgs e)
        {

            accessor = new DatabaseAccessor();
            accessor.ConnectToDatabase(Server.MapPath("/") + "FoodOrderingDB.mdb");

            List<Panel> panelsList = new List<Panel>();
            DataTable foodTable = accessor.GetItemList(ItemType.Drink);
            foreach (DataRow foodRow in foodTable.Rows)
            {
                Panel p = new Panel();
                p.ID = "pan" + foodRow.Field<int>("Drink_ID");
                p.CssClass = "MenuItem";

                Image i = new Image();
                i.ID = "img" + foodRow.Field<int>("Drink_ID");
                //i.ImageUrl = "~/Images/" + foodRow.Field<string>("Item_Image");
                i.AlternateText = "";
                i.ImageAlign = ImageAlign.Middle;
                i.CssClass = "MenuImage";
                Label l = new Label();
                l.ID = "lbl" + foodRow.Field<int>("Drink_ID");
                l.CssClass = "MenuLabel";
                l.Text = foodRow.Field<string>("Drink_Name") + " $" + foodRow.Field<decimal>("Drink_Cost");

                Button b = new Button();
                b.Click += Item_Click;
                b.ID = "btn" + foodRow.Field<int>("Drink_ID");
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
            Button b = sender as Button;

            if (Request.Cookies["Drink"] == null)
            {
                HttpCookie cookie = new HttpCookie("Drink", "DrinkItems");
                cookie.Values.Add("0", b.ID);
                Response.AppendCookie(cookie);
            }
            else
            {
                HttpCookie cookie = Request.Cookies["Drink"];
                cookie.Values.Add("" + cookie.Values.Count, b.ID);
                Response.SetCookie(cookie);
            }
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