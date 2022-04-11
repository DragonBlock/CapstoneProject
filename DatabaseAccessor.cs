using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using System.Drawing;
using System.Data;

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
            con.ConnectionString = @"Provider = Microsoft.Jet.OLEDB.4.0;" +
                                    @"Data Source = "+ path;
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
            try
            { con.Close(); }
            catch
            {

            }
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

        private string[] GetOrderDetailsColumnNames()
        {
            return new string[] {"OrderDetails_ID", "Order_ID", "Item_ID",
                "Side_ID", "Drink_ID", "Amount_Ordered", "Total_Cost"};
        }

        private bool IsConnectionOpen()
        {
            return con.State == System.Data.ConnectionState.Open;
        }

        public DataTable GetItemDetails(ItemType type, int itemID)
        {
            if (!IsConnectionOpen()) throw new Exception("Connection is Closed");

            DataTable searchResult;

            string query = "SELECT * FROM "+ GetItemTypeTableName(type) 
                + " WHERE " + GetItemTypeColumnNames(type)[0] + " = @value";
            OleDbCommand cmd = new OleDbCommand(query, con);
            cmd.Parameters.AddWithValue("@value", itemID);
            using (OleDbDataReader reader = cmd.ExecuteReader())
            {
                searchResult = new DataTable();

                searchResult.Load(reader);

                reader.Close();
            }

            return searchResult;
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

        public DataTable GetMenuSide(int itemID)
        {
            if (!IsConnectionOpen()) throw new Exception("Connection is Closed");

            int sideID = GetItemDetails(ItemType.Main, itemID).Rows[0].Field<int>("Item_Default_Side_ID");
            return GetItemDetails(ItemType.Side, sideID);
        }

        public DataTable GetItemList(ItemType type)
        {
            if (!IsConnectionOpen()) throw new Exception("Connection is Closed");

            DataTable searchResult;

            string query = "SELECT * FROM " + GetItemTypeTableName(type);
            OleDbCommand cmd = new OleDbCommand(query, con);

            using (OleDbDataReader reader = cmd.ExecuteReader())
            {
                searchResult = new DataTable();

                searchResult.Load(reader);

                reader.Close();
            }

            return searchResult;
        }

        public bool ValidateNewAccount(string newUsername)
        {
            List<Dictionary<string, string>> itemDetailsList = new List<Dictionary<string, string>>();

            string query = "SELECT Customer.Customer_Username, Employee.Employee_User_Name FROM Customer, Employee";
            OleDbCommand cmd = new OleDbCommand(query, con);

            List<string> takenUsernames = new List<string>();

            using (OleDbDataReader reader = cmd.ExecuteReader())
            {
                // Call Read before accessing data.
                while (reader.Read())
                {
                    takenUsernames.Add(reader["Customer_Username"].ToString());
                    takenUsernames.Add(reader["Employee_User_Name"].ToString());
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

            query = "SELECT Employee_ID, Employee_User_Name, Employee_Password FROM Employee";
            cmd.CommandText = query;

            using (OleDbDataReader reader = cmd.ExecuteReader())
            {
                // Call Read before accessing data.
                while (reader.Read())
                {
                    if (reader["Employee_User_Name"].ToString() == username
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

        public DataTable GetAccountDetails(AccountType aType, int userID)
        {
            DataTable accountDetails;

            if (aType == AccountType.Customer)
            {
                string query = "SELECT * FROM Customer" + 
                    " WHERE Customer_ID = @value";
                OleDbCommand cmd = new OleDbCommand(query, con);
                cmd.Parameters.AddWithValue("@value", userID);

                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    accountDetails = new DataTable();

                    accountDetails.Load(reader);

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
                    accountDetails = new DataTable();

                    accountDetails.Load(reader);

                    reader.Close();
                }
            }

            return accountDetails;
        }

        public bool ChangePassword(AccountType aType, int userID, string newPass)
        {
            if (!IsConnectionOpen()) throw new Exception("Connection is Closed");

            int worked = 0;

            if (aType == AccountType.Customer)
            {
                string query = "UPDATE Customer SET Customer_Password = '"+ newPass +"'" +
                    " WHERE Customer_ID = @ID";
                OleDbCommand cmd = new OleDbCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", userID);

                worked = cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            else
            {
                string query = "UPDATE Employee SET Employee_Password = '" + newPass + "'" +
                    " WHERE Employee_ID = @ID";
                OleDbCommand cmd = new OleDbCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", userID);

                worked = cmd.ExecuteNonQuery();
                cmd.Dispose();
            }

            if (worked == 0) return false;
            else return true;
        }

        public bool ChangeEmail(AccountType aType, int userID, string newEmail)
        {
            if (!IsConnectionOpen()) throw new Exception("Connection is Closed");

            int worked = 0;

            if (aType == AccountType.Customer)
            {
                string query = "UPDATE Customer SET Customer_Email = '" + newEmail + "'" +
                    " WHERE Customer_ID = @ID";
                OleDbCommand cmd = new OleDbCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", userID);

                worked = cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            else
            {
                string query = "UPDATE Employee SET Employee_Email = '" + newEmail + "'" +
                    " WHERE Employee_ID = @ID";
                OleDbCommand cmd = new OleDbCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", userID);

                worked = cmd.ExecuteNonQuery();
                cmd.Dispose();
            }

            if (worked == 0) return false;
            else return true;
        }

        public bool ChangeUsername(AccountType aType, int userID, string newUsername)
        {
            if (!IsConnectionOpen()) throw new Exception("Connection is Closed");

            if(!ValidateNewAccount(newUsername)) return false;

            int worked = 0;

            if (aType == AccountType.Customer)
            {
                string query = "UPDATE Customer SET Customer_Username = '" + newUsername + "'" +
                    " WHERE Customer_ID = @ID";
                OleDbCommand cmd = new OleDbCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", userID);

                worked = cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            else
            {
                string query = "UPDATE Employee SET Employee_User_Name = '" + newUsername + "'" +
                    " WHERE Employee_ID = @ID";
                OleDbCommand cmd = new OleDbCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", userID);

                worked = cmd.ExecuteNonQuery();
                cmd.Dispose();
            }

            if (worked == 0) return false;
            else return true;
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
                    total += GetItemDetails(ItemType.Main, orderDetails[i].menuID).Rows[0].Field<double>("Item_Price");
                }
                if (orderDetails[i].sideID != -1)
                {
                    total += GetItemDetails(ItemType.Side, orderDetails[i].sideID).Rows[0].Field<double>("Side_Price");
                }
                if (orderDetails[i].drinkID != -1)
                {
                    total += GetItemDetails(ItemType.Drink, orderDetails[i].menuID).Rows[0].Field<double>("Drink_Cost");
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

            string query = "INSERT INTO OrderTable (Customer_ID, Order_SubTotal, " +
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

            query = "SELECT Order_ID FROM OrderTable WHERE Customer_ID = @CustID AND Order_Date = @Date";
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

        public DataTable ViewOrderDetailsList(int orderID)
        {
            if (!IsConnectionOpen()) throw new Exception("Connection is Closed");

            DataTable orderDetailsTable;

            string query = "SELECT * FROM OrderDetails WHERE Order_ID = @OrderID";
            OleDbCommand cmd = new OleDbCommand(query, con);
            cmd.Parameters.AddWithValue("@OrderID", orderID);

            using (OleDbDataReader reader = cmd.ExecuteReader())
            {
                orderDetailsTable = new DataTable();

                orderDetailsTable.Load(reader);

                reader.Close();
            }

            return orderDetailsTable;
        }

        public DataTable GetItemTypes()
        {
            if (!IsConnectionOpen()) throw new Exception("Connection is Closed");

            DataTable itemTypesTable;

            string query = "SELECT * FROM ItemType";
            OleDbCommand cmd = new OleDbCommand(query, con);

            using (OleDbDataReader reader = cmd.ExecuteReader())
            {
                itemTypesTable = new DataTable();

                itemTypesTable.Load(reader);

                reader.Close();
            }

            return itemTypesTable;
        }

        public bool AddMenuItem(int typeID, string name, string desc, double price,
            int calories, string imageURL, bool hasDrink, string ingredients, int defaultSideID = -1)
        {
            if (!IsConnectionOpen()) throw new Exception("Connection is Closed");

            string query = "INSERT INTO Menu (Item_Type_ID, Item_Name, " +
                "Item_Description, Item_Price, Item_Calories, Item_Image, Item_Includes_Drink, "+
                (defaultSideID == -1 ? "Item_Default_Side_ID, " : "") + "Item_Ingredients) " +
                    "VALUES (@TypeID, @Name, @Desc, @Price, @Cals, @ImageURL, @HasDrink, " + 
                    (defaultSideID == -1  ? "@SideID, " : "") + "@Ingred)";
            OleDbCommand cmd = new OleDbCommand(query, con);
            DateTime time = DateTime.Now;
            cmd.Parameters.AddWithValue("@TypeID", typeID);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Desc", desc);
            cmd.Parameters.AddWithValue("@Price", price);
            cmd.Parameters.AddWithValue("@Cals", calories);
            cmd.Parameters.AddWithValue("@ImageURL", imageURL);
            cmd.Parameters.AddWithValue("@HasDrink", hasDrink);
            cmd.Parameters.AddWithValue("@SideID", defaultSideID);
            cmd.Parameters.AddWithValue("@Ingred", ingredients);

            int worked = cmd.ExecuteNonQuery();

            if (worked == 0) return false;
            else return true;
        }

        public bool AddSide(string name, string desc, double price, int calories)
        {
            if (!IsConnectionOpen()) throw new Exception("Connection is Closed");

            string query = "INSERT INTO SideMenu (Side_Name, Side_Description, " +
                "Side_Price, Side_Calories) " +
                    "VALUES (@Name, @Desc, @Price, @Cals)";
            OleDbCommand cmd = new OleDbCommand(query, con);
            DateTime time = DateTime.Now;
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Desc", desc);
            cmd.Parameters.AddWithValue("@Price", price);
            cmd.Parameters.AddWithValue("@Cals", calories);

            int worked = cmd.ExecuteNonQuery();

            if (worked == 0) return false;
            else return true;
        }

        public bool AddDrink(string name, string desc, double cost, bool costExtra)
        {
            if (!IsConnectionOpen()) throw new Exception("Connection is Closed");

            string query = "INSERT INTO DrinkMenu (Drink_Name, Drink_Description, " +
                "Drink_Cost, Drink_Costs_Extra) " +
                    "VALUES (@Name, @Desc, @Cost, @CostsExtra)";
            OleDbCommand cmd = new OleDbCommand(query, con);
            DateTime time = DateTime.Now;
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Desc", desc);
            cmd.Parameters.AddWithValue("@Cost", cost);
            cmd.Parameters.AddWithValue("@CostsExtra", costExtra);

            int worked = cmd.ExecuteNonQuery();

            if (worked == 0) return false;
            else return true;
        }

        public DataTable GetListOfOrders()
        {
            if (!IsConnectionOpen()) throw new Exception("Connection is Closed");

            DataTable ordersTable;

            string query = "SELECT Customer.Customer_ID, Customer.Customer_First_Name, Customer.Customer_Last_Name, OrderTable.Order_Total, " +
                "OrderTable.Order_Status, OrderTable.Order_Date, OrderTable.Order_Payment_Date, OrderTable.Order_Processed_By_Employee_ID, " +
                "OrderTable.Order_Payment_Amount, PaymentInfo.Info_Was_Cash, PaymentInfo.Info_Paid_In_Full " +
                "FROM (OrderTable INNER JOIN Customer ON Customer.Customer_ID = OrderTable.Customer_ID) " +
                "INNER JOIN PaymentInfo ON PaymentInfo.Payment_Info_ID = OrderTable.Payment_Info_ID";
            OleDbCommand cmd = new OleDbCommand(query, con);

            using (OleDbDataReader reader = cmd.ExecuteReader())
            {
                ordersTable = new DataTable();

                ordersTable.Load(reader);

                reader.Close();
            }

            return ordersTable;
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