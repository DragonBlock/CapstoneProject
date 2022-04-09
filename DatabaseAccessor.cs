using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using System.Drawing;


namespace CapstoneProject
{
    // Enums for Item & Account type indicators
    public enum ItemType { Main, Side, Drink }
    public enum AccountType { Customer, Employee }
    public class DatabaseAccessor
    {
        // Declare connection
        private OleDbConnection con;


        // Constructor
        public DatabaseAccessor()
        {
            // Initialize connection
            con = new OleDbConnection();
        }

        // Connection method
        public void ConnectToDatabase(string path)
        {
            con.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" +
                                    @"Data source=" + path;
            con.Open();
        }

        // Destructor
        ~DatabaseAccessor()
        {
            DisconnectFromDatabase();
            con.Dispose();
        }

        // Disconnection method
        public void DisconnectFromDatabase()
        {
            con.Close();
        }

        private string GetItemTypeTableName(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.Main:
                    return "Menu";
                case ItemType.Side:
                    return "SideMenu";
                case ItemType.Drink:
                    return "DrinkMenu";
                default:
                    return null;
            }
        }

        private string[] GetItemTypeColumnNames(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.Main:
                    return new string[] {"Item_ID", "Item_Type_ID", "Item_Name",
                        "Item_Description", "Item_Price", "Item_Calories", "Item_Image",
                        "Item_Includes_Drink", "Item_Default_Side_ID", "Item_Ingredients"};
                case ItemType.Side:
                    return new string[] {"Side_ID", "Side_Name", "Side_Description",
                        "Side_Price", "Side_Calories"};
                case ItemType.Drink:
                    return new string[] {"Drink_ID", "Drink_Name", "Drink_Description",
                        "Drink_Cost", "Drink_Costs_Extra"};
                default:
                    return null;
            }
        }


        private bool IsConnectionOpen()
        {
            return con.State == System.Data.ConnectionState.Open;
        }

        public Dictionary<string, string> GetItemDetails(ItemType type, int itemID)
        {
            if (!IsConnectionOpen()) throw new Exception("Connection is Closed");

            Dictionary<string, string> itemDetails = new Dictionary<string, string>();

            string query = "SELECT * FROM "+ GetItemTypeTableName(type) 
                + " WHERE " + GetItemTypeColumnNames(type)[0] + " = @value";
            OleDbCommand cmd = new OleDbCommand(query, con);
            cmd.Parameters.AddWithValue("@value", itemID);
            using (OleDbDataReader reader = cmd.ExecuteReader())
            {
                // Call Read before accessing data.
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        itemDetails.Add(GetItemTypeColumnNames(type)[i], reader[i].ToString());
                    }
                }

                // Call Close when done reading.
                reader.Close();
            }

            return itemDetails;
        }

        public Bitmap GetMenuItemImage(int itemID)
        {
            if (!IsConnectionOpen()) throw new Exception("Connection is Closed");

            string imageURL = "images/";

            string query = "SELECT Item_Image FROM Menu WHERE Item_ID = @value";
            OleDbCommand cmd = new OleDbCommand(query, con);
            cmd.Parameters.AddWithValue("@value", itemID);
            using (OleDbDataReader reader = cmd.ExecuteReader())
            {
                // Call Read before accessing data.
                while (reader.Read())
                {
                    imageURL += reader["Item_Image"];
                }

                // Call Close when done reading.
                reader.Close();
            }

            return new Bitmap(imageURL);
        }

        public void GetMenuSide(int itemID)
        {
            int sideID = Convert.ToInt32(GetItemDetails(ItemType.Main, itemID)["Item_Default_Side_ID"]);
            GetItemDetails(ItemType.Side, sideID);
        }

        public void GetMenuList()
        {

        }

        public void GetSideList()
        {

        }

        public void GetDrinkList()
        {

        }

        public bool ValidateNewAccount()
        {

        }

        public void CreateAccount()
        {

        }

        public bool ValidateLogin()
        {

        }

        public void GetAccountDetails(AccountType aType)
        {

        }

        public void PlaceOrder()
        {

        }

        public void ProcessPayment()
        {

        }

        public void ViewOrderDetailsList()
        {

        }

        public void AddMenuItem()
        {

        }

        public void AddDrink()
        {

        }

        public void AddSide()
        {

        }

        public void GetListOfOrders()
        {

        }


    }
}