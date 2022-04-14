using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CapstoneProject
{
    public partial class SeeCart : Page
    {
        DatabaseAccessor accessor;
        List<DataRow> items;
        List<DataRow> sides;
        List<DataRow> drinks;

        protected void Page_Load(object sender, EventArgs e)
        {
            accessor = new DatabaseAccessor();
            accessor.ConnectToDatabase(Server.MapPath("/") + "FoodOrderingDB.mdb");

            List<Panel> panelsList = new List<Panel>();

            items = new List<DataRow>();
            sides = new List<DataRow>();
            drinks = new List<DataRow>();

            decimal subTotal = 0;

            if (Request.Cookies["Menu"] != null)
            {
                HttpCookie menu = Request.Cookies["Menu"];
                if (menu.Values.Count != 0)
                {
                    foreach (string key in menu.Values.AllKeys)
                    {
                        if (menu.Values[key] != "MenuItems")
                        {
                            char[] itemChars = menu.Values[key].ToCharArray();
                            string idString = "";
                            for (int i = 0; i < itemChars.Length; i++)
                            {
                                if (Char.IsDigit(itemChars[i]))
                                {
                                    idString += itemChars[i];
                                }
                            }
                            int id = int.Parse(idString);

                            DataRow m = accessor.GetItemDetails(ItemType.Main, id).Rows[0];

                            subTotal += m.Field<decimal>("Item_Price");

                            items.Add(m);
                        }
                    }
                }
            }
            if (Request.Cookies["Side"] != null)
            {
                HttpCookie side = Request.Cookies["Side"];
                foreach (string key in side.Values.AllKeys)
                {
                    if (side.Values[key] != "SideItems")
                    {
                        char[] itemChars = side.Values[key].ToCharArray();
                        string idString = "";
                        for (int i = 0; i < itemChars.Length; i++)
                        {
                            if (Char.IsDigit(itemChars[i]))
                            {
                                idString += itemChars[i];
                            }
                        }
                        int id = int.Parse(idString);

                        DataRow s = accessor.GetItemDetails(ItemType.Side, id).Rows[0];

                        subTotal += s.Field<decimal>("Side_Price");

                        sides.Add(s);
                    }
                }
            }
            if (Request.Cookies["Drink"] != null)
            {
                HttpCookie drink = Request.Cookies["Drink"];
                foreach (string key in drink.Values.AllKeys)
                {
                    if (drink.Values[key] != "DrinkItems")
                    {
                        char[] itemChars = drink.Values[key].ToCharArray();
                        string idString = "";
                        for (int i = 0; i < itemChars.Length; i++)
                        {
                            if (Char.IsDigit(itemChars[i]))
                            {
                                idString += itemChars[i];
                            }
                        }
                        int id = int.Parse(idString);

                        DataRow d = accessor.GetItemDetails(ItemType.Drink, id).Rows[0];

                        subTotal += d.Field<decimal>("Drink_Cost");

                        drinks.Add(d);
                    }
                }
            }

            // Menu Meals
            foreach (DataRow foodRow in items)
            {
                Panel p = new Panel();
                p.ID = "panItem" + foodRow.Field<int>("Item_ID");
                p.CssClass = "MenuItem";

                Image i = new Image();
                i.ID = "imgItem" + foodRow.Field<int>("Item_ID");
                i.ImageUrl = "~/Images/" + foodRow.Field<string>("Item_Image");
                i.ImageAlign = ImageAlign.Middle;
                i.CssClass = "MenuImage";
                Label l = new Label();
                l.ID = "lblItem" + foodRow.Field<int>("Item_ID");
                l.CssClass = "MenuLabel";
                l.Text = foodRow.Field<string>("Item_Name") + " $" + foodRow.Field<decimal>("Item_Price");

                Button b = new Button();
                b.Click += Item_Click;
                b.ID = "btnItem" + foodRow.Field<int>("Item_ID");
                b.CssClass = "MenuButton";
                b.Text = "Remove From Order";


                p.Controls.Add(i);
                p.Controls.Add(l);
                p.Controls.Add(b);

                lock (Controls.SyncRoot)
                {
                    MainPanel.Controls.Add(p);
                }

                panelsList.Add(p);
            }

            // Menu Sides
            foreach (DataRow foodRow in sides)
            {
                Panel p = new Panel();
                p.ID = "panSide" + foodRow.Field<int>("Side_ID");
                p.CssClass = "MenuItem";

                Image i = new Image();
                i.ID = "imgSide" + foodRow.Field<int>("Side_ID");
                DataRow main = accessor.GetMenuWithSide(foodRow.Field<int>("Side_ID"));
                if (main != null)
                {
                    i.ImageUrl = "~/Images/" + main.Field<string>("Item_Image");
                }
                i.AlternateText = "";
                i.ImageAlign = ImageAlign.Middle;
                i.CssClass = "MenuImage";
                Label l = new Label();
                l.ID = "lblSide" + foodRow.Field<int>("Side_ID");
                l.CssClass = "MenuLabel";
                l.Text = foodRow.Field<string>("Side_Name") + " $" + foodRow.Field<decimal>("Side_Price");

                Button b = new Button();
                b.Click += Item_Click;
                b.ID = "btnSide" + foodRow.Field<int>("Side_ID");
                b.CssClass = "MenuButton";
                b.Text = "Remove From Order";


                p.Controls.Add(i);
                p.Controls.Add(l);
                p.Controls.Add(b);

                lock (Controls.SyncRoot)
                {
                    MainPanel.Controls.Add(p);
                }

                panelsList.Add(p);
            }

            // Menu Drinks
            foreach (DataRow foodRow in drinks)
            {
                Panel p = new Panel();
                p.ID = "panDrink" + foodRow.Field<int>("Drink_ID");
                p.CssClass = "MenuItem";

                Image i = new Image();
                i.ID = "imgDrink" + foodRow.Field<int>("Drink_ID");
                //i.ImageUrl = "~/Images/" + foodRow.Field<string>("Item_Image");
                i.AlternateText = "";
                i.ImageAlign = ImageAlign.Middle;
                i.CssClass = "MenuImage";
                Label l = new Label();
                l.ID = "lblDrink" + foodRow.Field<int>("Drink_ID");
                l.CssClass = "MenuLabel";
                l.Text = foodRow.Field<string>("Drink_Name") + " $" + foodRow.Field<decimal>("Drink_Cost");

                Button b = new Button();
                b.Click += Item_Click;
                b.ID = "btnDrink" + foodRow.Field<int>("Drink_ID");
                b.CssClass = "MenuButton";
                b.Text = "Remove From Order";


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

            if (b.ID.Contains("Item"))
            {
                if (Request.Cookies["Menu"] != null)
                {
                    HttpCookie cookie = Request.Cookies["Menu"];
                    string idString = "";
                    for (int i = 0; i < b.ID.Length; i++)
                    {
                        if (Char.IsDigit(b.ID[i]))
                        {
                            idString += b.ID[i];
                        }
                    }
                    int itemID = int.Parse(idString);
                
                    foreach (string key in cookie.Values.AllKeys)
                    {
                        if (cookie.Values[key] != "MenuItems")
                        {
                            string val = cookie.Values[key];
                            idString = "";
                            for (int i = 0; i < val.Length; i++)
                            {
                                if (Char.IsDigit(val[i]))
                                {
                                    idString += val[i];
                                }
                            }
                            int id = int.Parse(idString);


                            if (itemID == id)
                            {
                                cookie.Values.Remove(key);
                                Response.SetCookie(cookie);
                                Response.Redirect("SeeCart.aspx");
                                return;
                            }
                        }
                    }
                }
            }
            else if (b.ID.Contains("Side"))
            {
                if (Request.Cookies["Side"] != null)
                {
                    HttpCookie cookie = Request.Cookies["Side"];
                    string idString = "";
                    for (int i = 0; i < b.ID.Length; i++)
                    {
                        if (Char.IsDigit(b.ID[i]))
                        {
                            idString += b.ID[i];
                        }
                    }
                    int sideID = int.Parse(idString);

                    foreach (string key in cookie.Values.AllKeys)
                    {
                        if (cookie.Values[key] != "SideItems")
                        {
                            string val = cookie.Values[key];
                            idString = "";
                            for (int i = 0; i < val.Length; i++)
                            {
                                if (Char.IsDigit(val[i]))
                                {
                                    idString += val[i];
                                }
                            }
                            int id = int.Parse(idString);

                            if (sideID == id)
                            {
                                cookie.Values.Remove(key);
                                Response.SetCookie(cookie);
                                Response.Redirect("SeeCart.aspx");
                                return;
                            }
                        }
                    }
                }
            }
            else if (b.ID.Contains("Drink"))
            {
                if (Request.Cookies["Drink"] != null)
                {
                    HttpCookie cookie = Request.Cookies["Drink"];
                    string idString = "";
                    for (int i = 0; i < b.ID.Length; i++)
                    {
                        if (Char.IsDigit(b.ID[i]))
                        {
                            idString += b.ID[i];
                        }
                    }
                    int drinkID = int.Parse(idString);

                    foreach (string key in cookie.Values.AllKeys)
                    {
                        if (cookie.Values[key] != "DrinkItems")
                        {
                            string val = cookie.Values[key];
                            idString = "";
                            for (int i = 0; i < val.Length; i++)
                            {
                                if (Char.IsDigit(val[i]))
                                {
                                    idString += val[i];
                                }
                            }
                            int id = int.Parse(idString);

                            if (drinkID == id)
                            {
                                cookie.Values.Remove(key);
                                Response.SetCookie(cookie);
                                Response.Redirect("SeeCart.aspx");
                                return;
                            }
                        }
                    }
                }
            }

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