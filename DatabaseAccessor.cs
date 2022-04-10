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

        public Dictionary<string, string> GetMenuSide(int itemID)
        {
            if (!IsConnectionOpen()) throw new Exception("Connection is Closed");

            int sideID = Convert.ToInt32(GetItemDetails(ItemType.Main, itemID)["Item_Default_Side_ID"]);
            return GetItemDetails(ItemType.Side, sideID);
        }

        public List<Dictionary<string, string>> GetItemList(ItemType type)
        {
            if (!IsConnectionOpen()) throw new Exception("Connection is Closed");

            List<Dictionary<string, string>> itemDetailsList = new List<Dictionary<string, string>>();

            string query = "SELECT * FROM " + GetItemTypeTableName(type);
            OleDbCommand cmd = new OleDbCommand(query, con);

            using (OleDbDataReader reader = cmd.ExecuteReader())
            {
                // Call Read before accessing data.
                while (reader.Read())
                {
                    Dictionary<string, string> itemDetails = new Dictionary<string, string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        itemDetails.Add(GetItemTypeColumnNames(type)[i], reader[i].ToString());
                    }
                    itemDetailsList.Add(itemDetails);
                }

                // Call Close when done reading.
                reader.Close();
            }

            return itemDetailsList;
        }

        public bool ValidateNewAccount(string newUsername)
        {
            List<Dictionary<string, string>> itemDetailsList = new List<Dictionary<string, string>>();

            string query = "SELECT Customer.Customer_Username, Employee.Employee_Username FROM Customer, Employee";
            OleDbCommand cmd = new OleDbCommand(query, con);

            List<string> takenUsernames = new List<string>();

            using (OleDbDataReader reader = cmd.ExecuteReader())
            {
                // Call Read before accessing data.
                while (reader.Read())
                {
                    takenUsernames.Add(reader["Customer_Username"].ToString());
                    takenUsernames.Add(reader["Employee_Username"].ToString());
                }

                // Call Close when done reading.
                reader.Close();
            }

            for (int i = 0; i < takenUsernames.Count; i++)
            {
                if (newUsername == takenUsernames[i]) return false;
            }

            return true;
        }

        public bool CreateAccount(string username, string password, string fName, string lName, string email, string phoneNum)
        {
            if (!IsConnectionOpen()) throw new Exception("Connection is Closed");

            if(!ValidateNewAccount(username)) return false;

            string query = "INSERT INTO INSERT INTO Customer (Customer_Username, Customer_Password, Customer_First_Name, Customer_Last_Name, Customer_Email, Customer_Phone_Num) " +
                    "VALUES (@Username, @Pass, @FirstName, @LastName, @Email, @Phone)";
            OleDbCommand cmd = new OleDbCommand(query, con);
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Pass", password);
            cmd.Parameters.AddWithValue("@FirstName", fName);
            cmd.Parameters.AddWithValue("@LastName", lName);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Phone", phoneNum);

            int worked = cmd.ExecuteNonQuery();

            if (worked == 0) return false;
            else return true;
        }

        public KeyValuePair<AccountType, int> ValidateLogin(string username, string password)
        {
            // Return -1 if not valid

            string query = "SELECT Customer_ID, Customer_Username, Customer_Password FROM Customer";
            OleDbCommand cmd = new OleDbCommand(query, con);

            using (OleDbDataReader reader = cmd.ExecuteReader())
            {
                // Call Read before accessing data.
                while (reader.Read())
                {
                    if (reader["Customer_Username"].ToString() == username
                        && reader["Customer_Password"].ToString() == password)
                    {
                        return new KeyValuePair<AccountType, int>(AccountType.Customer,Convert.ToInt32(reader["Customer_ID"]));
                    }
                }

                // Call Close when done reading.
                reader.Close();
            }

            query = "SELECT Employee_ID, Employee_Username, Employee_Password FROM Employee";
            cmd.CommandText = query;
            using (OleDbDataReader reader = cmd.ExecuteReader())
            {
                // Call Read before accessing data.
                while (reader.Read())
                {
                    if (reader["Employee_Username"].ToString() == username
                        && reader["Employee_Password"].ToString() == password)
                    {
                        return new KeyValuePair<AccountType, int>(AccountType.Employee, Convert.ToInt32(reader["Employee_ID"]));
                    }
                }

                // Call Close when done reading.
                reader.Close();
            }

            return new KeyValuePair<AccountType, int>(AccountType.Customer, -1);
        }

        public void GetAccountDetails(AccountType aType, int userID)
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