using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;


namespace CapstoneProject
{
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

        // Connection Method
        public void ConnectToDatabase(string path)
        {
            con.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" +
                                    @"Data source=" + path;
            con.Open();
        }

        public void GetMenuImage()
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

        public void ValidateLogin()
        {

        }

        public void PlaceOrder()
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


    }
}