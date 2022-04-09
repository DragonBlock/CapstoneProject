using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;


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


        public void GetItemDetails(ItemType type)
        {

        }

        public void GetItemImage(ItemType type)
        {

        }

        public void GetMenuSide()
        {

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

        public void GetAccountDetails()
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