using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CapstoneProject
{
    public partial class MenuSides : Page
    {
        DatabaseAccessor accessor;

        protected void Page_Load(object sender, EventArgs e)
        {
            accessor = new DatabaseAccessor();
            accessor.ConnectToDatabase(Server.MapPath("/") + "FoodOrderingDB.mdb");

            List<Panel> panelsList = new List<Panel>();
            DataTable foodTable = accessor.GetItemList(ItemType.Side);
            foreach (DataRow foodRow in foodTable.Rows)
            {
                Panel p = new Panel();
                p.ID = "pan" + foodRow.Field<int>("Side_ID");
                p.CssClass = "MenuItem";

                Image i = new Image();
                i.ID = "img" + foodRow.Field<int>("Side_ID");
                //i.ImageUrl = "~/Images/" + foodRow.Field<string>("Item_Image");
                i.AlternateText = "";
                i.ImageAlign = ImageAlign.Middle;
                i.CssClass = "MenuImage";
                Label l = new Label();
                l.ID = "lbl" + foodRow.Field<int>("Side_ID");
                l.CssClass = "MenuLabel";
                l.Text = foodRow.Field<string>("Side_Name") + " $" + foodRow.Field<decimal>("Side_Price");

                Button b = new Button();
                b.Click += Item_Click;
                b.ID = "btn" + foodRow.Field<int>("Side_ID");
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

            if (Request.Cookies["Side"] == null)
            {
                HttpCookie cookie = new HttpCookie("Side", "SideItems");
                cookie.Values.Add("0", b.ID);
                Response.AppendCookie(cookie);
            }
            else
            {
                HttpCookie cookie = Request.Cookies["Side"];
                cookie.Values.Add("" + cookie.Values.Count, b.ID);
                Response.SetCookie(cookie);
            }
        }

        protected void btnMeals_Click(object sender, EventArgs e)
        {
            Response.Redirect("MenuMeals.aspx");
        }

        protected void btnDrinks_Click(object sender, EventArgs e)
        {
            Response.Redirect("MenuDrinks.aspx");
        }
    }
}