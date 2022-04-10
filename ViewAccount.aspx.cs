using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CapstoneProject
{
    public partial class ViewAccount : System.Web.UI.Page
    {
        DatabaseAccessor accessor;

        protected void Page_Load(object sender, EventArgs e)
        {
            accessor = new DatabaseAccessor();
            accessor.ConnectToDatabase(Server.MapPath("/")+ "FoodOrderingDB.mdb");
            string acc = Request.Cookies["LoginInfo"].Value;
            AccountType aType;
            if (acc.Contains("Customer")) aType = AccountType.Customer;
            else aType = AccountType.Employee;
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
            DataTable accTable = accessor.GetAccountDetails(aType, id);
            DataRow accDetails = accTable.Rows[0];
            string username = "";
            string email = "";
            if (aType == AccountType.Customer)
            {
                username = accDetails.Field<string>("Customer_Username");
                email = accDetails.Field<string>("Customer_Email");
            }
            else
            {
                username = accDetails.Field<string>("Employee_User_Name");
                email = accDetails.Field<string>("Employee_Email");
            }

            lblUsername.Text = username;
            lblEmail.Text = email;
        }

        protected void btnChangePass_Click(object sender, EventArgs e)
        {
            string acc = Request.Cookies["LoginInfo"].Value;
            AccountType aType;
            if (acc.Contains("Customer")) aType = AccountType.Customer;
            else aType = AccountType.Employee;
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
            DataTable accTable = accessor.GetAccountDetails(aType, id);
            DataRow accDetails = accTable.Rows[0];
            string prevPass = "";
            if (aType == AccountType.Customer)
            {
                prevPass = accDetails.Field<string>("Customer_Password");
            }
            else
            {
                prevPass = accDetails.Field<string>("Employee_Password");
            }

            if (tbOldPass.Text.ToString() != prevPass)
            {
                Label1.Text = "Wrong current password.  Please try again.";
            }
            else if (tbNewPass.Text.ToString().Contains(" "))
            {
                Label1.Text = "Cannot have spaces in password.  Please try again.";
            }
            else if (tbOldPass.Text.ToString() == tbNewPass.Text.ToString())
            {
                Label1.Text = "Old password and new password must be different.  Please try again.";
            }
            else
            {
                bool worked = accessor.ChangePassword(aType, id, tbNewPass.Text.ToString());
                if (worked) Label1.Text = "Password changed.";
                else Label1.Text = "Invalid password.";
            }
        }

        protected void btnNewEmail_Click(object sender, EventArgs e)
        {
            string acc = Request.Cookies["LoginInfo"].Value;
            AccountType aType;
            if (acc.Contains("Customer")) aType = AccountType.Customer;
            else aType = AccountType.Employee;
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
            DataTable accTable = accessor.GetAccountDetails(aType, id);
            DataRow accDetails = accTable.Rows[0];
            string prevEmail = "";
            if (aType == AccountType.Customer)
            {
                prevEmail = accDetails.Field<string>("Customer_Email");
            }
            else
            {
                prevEmail = accDetails.Field<string>("Employee_Email");
            }

            if (tbEmail.Text.ToString() == prevEmail)
            {
                Label2.Text = "New email must be different.  Please try again.";
            }
            else if (!tbEmail.Text.ToString().Contains("@") || !tbEmail.Text.ToString().Contains("."))
            {
                Label2.Text = "Invalid email.  Please try again.";
            }
            else
            {
                bool worked = accessor.ChangeEmail(aType, id, tbEmail.Text.ToString());
                if (worked) Label2.Text = "Email changed.";
                else Label2.Text = "Invalid email.";
            }
        }

        protected void btnNewUsername_Click(object sender, EventArgs e)
        {
            string acc = Request.Cookies["LoginInfo"].Value;
            AccountType aType;
            if (acc.Contains("Customer")) aType = AccountType.Customer;
            else aType = AccountType.Employee;
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
            DataTable accTable = accessor.GetAccountDetails(aType, id);
            DataRow accDetails = accTable.Rows[0];
            string prevUsername = "";
            if (aType == AccountType.Customer)
            {
                prevUsername = accDetails.Field<string>("Customer_Username");
            }
            else
            {
                prevUsername = accDetails.Field<string>("Employee_User_Name");
            }

            if (tbEmail.Text.ToString() == prevUsername)
            {
                Label3.Text = "New username must be different.  Please try again.";
            }
            else if (tbEmail.Text.ToString().Contains(" "))
            {
                Label3.Text = "Invalid username.  Please try again.";
            }
            else
            {
                bool worked = accessor.ChangeUsername(aType, id, tbNewUser.Text.ToString());
                if (worked) Label3.Text = "Username changed.";
                else Label3.Text = "Username already taken.";
            }
        }
    }
}