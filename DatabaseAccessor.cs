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

        private string[] GetAccountTypeColumnNames(AccountType accountType)
        {
            switch (accountType)
            {
                case AccountType.Customer:
                    return new string[] {"Customer_ID", "Customer_First_Name", "Customer_Last_Name",
                        "Customer_Email", "Customer_Phone_Num", "Customer_Username", "Customer_Password"};
                case AccountType.Employee:
                    return new string[] {"Employee_ID", "Employee_First_Name", "Employee_Last_Name",
                        "Employee_Email", "Employee_User_Name", "Employee_Password",
                    "Employee_Has_Menu_Privileges", "Employee_Has_Payment_Privileges"};
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

            string query = "INSERT INTO Customer (Customer_Username, Customer_Password, Customer_First_Name, Customer_Last_Name, Customer_Email, Customer_Phone_Num) " +
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

        public Dictionary<string, string> GetAccountDetails(AccountType aType, int userID)
        {
            Dictionary<string, string> accountDetails = new Dictionary<string, string>();

            if (aType == AccountType.Customer)
            {
                string query = "SELECT * FROM Customer" + 
                    " WHERE Customer_ID = @value";
                OleDbCommand cmd = new OleDbCommand(query, con);
                cmd.Parameters.AddWithValue("@value", userID);

                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    // Call Read before accessing data.
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            accountDetails.Add(GetAccountTypeColumnNames(aType)[i], reader[i].ToString());
                        }
                    }

                    // Call Close when done reading.
                    reader.Close();
                }
            }
            else
            {
                string query = "SELECT * FROM Employee" +
                    " WHERE Employee_ID = @value";
                OleDbCommand cmd = new OleDbCommand(query, con);
                cmd.Parameters.AddWithValue("@value", userID);

                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    // Call Read before accessing data.
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            accountDetails.Add(GetAccountTypeColumnNames(aType)[i], reader[i].ToString());
                        }
                    }

                    // Call Close when done reading.
                    reader.Close();
                }
            }

            return accountDetails;
        }

        public bool PlaceOrder(int custID, List<OrderDetail> orderDetails, bool isCash, PaymentInfo info)
        {
            if (!IsConnectionOpen()) throw new Exception("Connection is Closed");

            int paymentID = ProcessPayment(isCash, info);

            if (paymentID == -1) return false;

            List<double> totals = new List<double>();

            for (int i = 0; i < orderDetails.Count; i++)
            {
                double total = 0;
                if (orderDetails[i].menuID != -1)
                {
                    total += double.Parse(GetItemDetails(ItemType.Main, orderDetails[i].menuID)["Item_Price"]);
                }
                if (orderDetails[i].sideID != -1)
                {
                    total += double.Parse(GetItemDetails(ItemType.Side, orderDetails[i].sideID)["Side_Price"]);
                }
                if (orderDetails[i].drinkID != -1)
                {
                    total += double.Parse(GetItemDetails(ItemType.Drink, orderDetails[i].menuID)["Drink_Cost"]);
                }
                total *= orderDetails[i].amount;
                totals.Add(total);
            }

            double orderSubtotal = 0;
            for (int i = 0; i < totals.Count; i++)
            {
                orderSubtotal += totals[i];
            }
            double orderTotal = Math.Round(orderSubtotal * 1.06, 2);

            string date = "";
            string dateVal = "";
            if (!isCash)
            {
                date = "Order_Payment_Date, ";
                dateVal = ", @PayDate";
            }

            string query = "INSERT INTO Order (Customer_ID, Order_SubTotal, "+
                "Order_Total, Order_Status, Order_Date, Payment_Info_ID, " + date + ") " +
                    "VALUES (@CustID, @SubTotal, @Total, @Status, @Date, @PaymentID" + dateVal + ")";
            OleDbCommand cmd = new OleDbCommand(query, con);
            DateTime time = DateTime.Now;
            cmd.Parameters.AddWithValue("@CustID", custID);
            cmd.Parameters.AddWithValue("@SubTotal", orderSubtotal);
            cmd.Parameters.AddWithValue("@Total", orderTotal);
            cmd.Parameters.AddWithValue("@Status", isCash ? "Paid" : "Ordered");
            cmd.Parameters.AddWithValue("@Date", time);
            cmd.Parameters.AddWithValue("@PaymentID", paymentID);
            cmd.Parameters.AddWithValue("@PayDate", time);

            int worked = cmd.ExecuteNonQuery();

            query = "SELECT Order_ID FROM Order WHERE Customer_ID = @CustID AND Order_Date = @Date";
            cmd.CommandText = query;

            int orderID = -1;

            using (OleDbDataReader reader = cmd.ExecuteReader())
            {
                // Call Read before accessing data.
                while (reader.Read())
                {
                    orderID = int.Parse(reader["Order_ID"].ToString());
                }

                // Call Close when done reading.
                reader.Close();
            }

            for (int i = 0; i < orderDetails.Count; i++)
            {
                query = "INSERT INTO OrderDetails (Order_ID, ";
                string vals = "VALUES (@OrderID, ";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@OrderID", orderID);
                if (orderDetails[i].menuID != -1)
                {
                    query += "Item_ID, ";
                    vals += "@ItemID, ";
                    cmd.Parameters.AddWithValue("@ItemID", orderDetails[i].menuID);
                }
                if (orderDetails[i].sideID != -1)
                {
                    query += "Side_ID, ";
                    vals += "@SideID, ";
                    cmd.Parameters.AddWithValue("@SideID", orderDetails[i].sideID);
                }
                if (orderDetails[i].drinkID != -1)
                {
                    query += "Drink_ID, ";
                    vals += "@DrinkID, ";
                    cmd.Parameters.AddWithValue("@DrinkID", orderDetails[i].drinkID);
                }

                query = "Amount_Ordered, Total_Cost) " +
                    vals + " @Amount, @Total)";
                cmd.CommandText = query;
               
                cmd.Parameters.AddWithValue("@Amount", "Ordered");
                cmd.Parameters.AddWithValue("@Total", DateTime.Now);

                int executed = cmd.ExecuteNonQuery();
                if (executed == 0) return false;
            }

            if (worked == 0) return false;
            else return true;
        }

        public int ProcessPayment(bool isCash, PaymentInfo info)
        {
            string query = "INSERT INTO PaymentInfo (Info_Was_Cash, " +
                "Info_Address, Info_City, Info_Zip_Code, Info_State, Info_Country, Info_Paid_In_Full) " +
                    "VALUES (@IsCash, @Address, @City, @Zip, @State, @Country, @Paid)";
            OleDbCommand cmd = new OleDbCommand(query, con);
            cmd.Parameters.AddWithValue("@IsCash", isCash);
            cmd.Parameters.AddWithValue("@Address", info.address);
            cmd.Parameters.AddWithValue("@City", info.city);
            cmd.Parameters.AddWithValue("@Zip", info.zipcode);
            cmd.Parameters.AddWithValue("@State", info.state);
            cmd.Parameters.AddWithValue("@Country", info.country);
            cmd.Parameters.AddWithValue("@Paid", !isCash);

            int worked = cmd.ExecuteNonQuery();

            if (worked == 0) return -1;

            query = "SELECT Payment_Info_ID FROM PaymentInfo WHERE Info_Address = @Address AND Info_City = @City" +
                " AND Info_Zip_Code = @Zip AND Info_State = @State AND Info_Country = @Country";
            cmd.CommandText = query;

            int paymentID = -1;

            using (OleDbDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    paymentID = int.Parse(reader["Payment_Info_ID"].ToString());
                }

                reader.Close();
            }
            return paymentID;
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
    public struct OrderDetail
    {
        OrderDetail(int menuid = -1, int sideid = -1, int drinkid = -1, int amt = 1)
        {
            menuID = menuid;
            sideID = sideid;
            drinkID = drinkid;
            amount = amt;
        }
        public int menuID { get; set; }
        public int sideID { get; set; }
        public int drinkID { get; set; }
        public int amount { get; set; }
    }
    public struct PaymentInfo
    {
        PaymentInfo(string add, string c, string zip, string s, string cy)
        {
            address = add;
            city = c;
            zipcode = zip;
            state = s;
            country = cy;
        }
        public string address { get; set; }
        public string city { get; set; }
        public string zipcode { get; set; }
        public string state { get; set; }
        public string country { get; set; }
    }
}