using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace CapstoneProject
{
    public partial class Checkout : Page
    {
        DatabaseAccessor accessor;
        List<DataRow> items;
        List<DataRow> sides;
        List<DataRow> drinks;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["LoginInfo"] == null)
            {
                btnLogin.Visible = true;
                btnLogin.Enabled = true;
                btnCheckout.Visible = false;
                btnCheckout.Enabled = false;

            }
            else if (Request.Cookies["LoginInfo"].Value.Contains("Customer"))
            {
                btnLogin.Visible = false;
                btnLogin.Enabled = false;
                btnCheckout.Visible = true;
                btnCheckout.Enabled = true;
            }
            else
            {
                btnLogin.Visible = true;
                btnLogin.Enabled = true;
                btnCheckout.Visible = false;
                btnCheckout.Enabled = false;
            }

            accessor = new DatabaseAccessor();
            accessor.ConnectToDatabase(Server.MapPath("/") + "FoodOrderingDB.mdb");

            items = new List<DataRow>();
            sides = new List<DataRow>();
            drinks = new List<DataRow>();

            decimal subTotal = 0;

            tbOrder.Text = "";

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
                            tbOrder.Text += "\n" + m.Field<string>("Item_Name") + " $" + m.Field<decimal>("Item_Price");

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
                        tbOrder.Text += "\n" + s.Field<string>("Side_Name") + " $" + s.Field<decimal>("Side_Price");

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
                        tbOrder.Text += "\n" + d.Field<string>("Drink_Name") + " $" + d.Field<decimal>("Drink_Cost");

                        subTotal += d.Field<decimal>("Drink_Cost");

                        drinks.Add(d);
                    }
                }
            }
            tbOrder.Text += "\n\nSubtotal: $" + subTotal + 
                "\nTax: $"+ Math.Round(((double)(subTotal) * 0.06), 2) + "\nTotal: $" + Math.Round(((double)(subTotal) * 1.06), 2);

        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            if (txtAddress.Text == "" || txtCity.Text == "" || txtZip.Text == "" || txtState.Text == "" || txtCountry.Text == "")
            {
                message.Text = "Not all address info filled out";
            }
            else if (items.Count == 0 && sides.Count == 0 && drinks.Count == 0)
            {
                message.Text = "No items to purchase.";
            }
            else
            {
                string acc = Request.Cookies["LoginInfo"].Value;
                char[] accChars = acc.ToCharArray();
                string idString = "";
                for (int i = 0; i < accChars.Length; i++)
                {
                    if (Char.IsDigit(accChars[i]))
                    {
                        idString += accChars[i];
                    }
                }
                int id = int.Parse(idString);
                List<OrderDetail> orderList = new List<OrderDetail>();
                foreach (DataRow i in items)
                {
                    OrderDetail o = new OrderDetail(i.Field<int>("Item_ID"));
                    orderList.Add(o);
                }
                foreach (DataRow s in sides)
                {
                    OrderDetail o = new OrderDetail(s.Field<int>("Item_ID"));
                    orderList.Add(o);
                }
                foreach (DataRow d in drinks)
                {
                    OrderDetail o = new OrderDetail(d.Field<int>("Item_ID"));
                    orderList.Add(o);
                }
                bool placed = accessor.PlaceOrder(id, orderList, ddlCash.SelectedValue == "Cash", 
                    new PaymentInfo(txtAddress.Text, txtCity.Text, txtZip.Text, txtState.Text, txtCountry.Text));
                if (placed)
                {
                    message.Text = "Order Placed.";

                    HttpCookie currentUserCookie = HttpContext.Current.Request.Cookies["Menu"];
                    HttpContext.Current.Response.Cookies.Remove("Menu");
                    currentUserCookie.Expires = DateTime.Now.AddDays(-10);
                    currentUserCookie.Value = null;
                    HttpContext.Current.Response.SetCookie(currentUserCookie);

                    currentUserCookie = HttpContext.Current.Request.Cookies["Side"];
                    HttpContext.Current.Response.Cookies.Remove("Side");
                    currentUserCookie.Expires = DateTime.Now.AddDays(-10);
                    currentUserCookie.Value = null;
                    HttpContext.Current.Response.SetCookie(currentUserCookie);

                    currentUserCookie = HttpContext.Current.Request.Cookies["Drink"];
                    HttpContext.Current.Response.Cookies.Remove("Drink");
                    currentUserCookie.Expires = DateTime.Now.AddDays(-10);
                    currentUserCookie.Value = null;
                    HttpContext.Current.Response.SetCookie(currentUserCookie);

                    Request.Cookies.Clear();
                    Response.Redirect("ViewAccount.aspx");
                }
                else message.Text = "Order Failed.";
            }
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }

        protected void Button2_Click(object sender, EventArgs e)
        {

        }

        
    }
}