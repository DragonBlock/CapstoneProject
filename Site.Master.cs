using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Speech;
using System.Speech.Synthesis;
using System.IO;
using System.Threading.Tasks;

namespace CapstoneProject
{
    public partial class SiteMaster : MasterPage
    {
        public DatabaseAccessor accessor;

        string textToSay = "hi";
        string fileName = "speak";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies.Count == 0)
            {
                btnAdmin0.Visible = false;
                btnAdmin0.Enabled = false;
                btnAccount0.Visible = false;
                btnAccount0.Enabled = false;
                btnLogin0.Visible = true;
                btnLogin0.Enabled = true;
                btnSignUp0.Visible = true;
                btnSignUp0.Enabled = true;
                btnLogOut.Visible = false;
                btnLogOut.Enabled = false;

            }
            else if (Request.Cookies["LoginInfo"].Value.Contains("Customer"))
            {
                btnAdmin0.Visible = false;
                btnAdmin0.Enabled = false;
                btnAccount0.Visible = true;
                btnAccount0.Enabled = true;
                btnLogin0.Visible = false;
                btnLogin0.Enabled = false;
                btnSignUp0.Visible = false;
                btnSignUp0.Enabled = false;
                btnLogOut.Visible = true;
                btnLogOut.Enabled = true;
            }
            else if (Request.Cookies["LoginInfo"].Value.Contains("Employee"))
            {
                btnAdmin0.Visible = true;
                btnAdmin0.Enabled = true;
                btnAccount0.Visible = false;
                btnAccount0.Enabled = false;
                btnLogin0.Visible = false;
                btnLogin0.Enabled = false;
                btnSignUp0.Visible = false;
                btnSignUp0.Enabled = false;
                btnLogOut.Visible = true;
                btnLogOut.Enabled = true;
            }
            else
            {
                btnAdmin0.Visible = false;
                btnAdmin0.Enabled = false;
                btnAccount0.Visible = false;
                btnAccount0.Enabled = false;
                btnLogin0.Visible = true;
                btnLogin0.Enabled = true;
                btnSignUp0.Visible = true;
                btnSignUp0.Enabled = true;
                btnLogOut.Visible = false;
                btnLogOut.Enabled = false;
            }
            accessor = new DatabaseAccessor();
            accessor.ConnectToDatabase(Server.MapPath("/") + "FoodOrderingDB.mdb");



            bool play = true;
            switch (this.Page.Title) {
                //case "Home Page":
                //    textToSay = "Welcome to the home page!";
                //    break;
                case "MenuMeals":
                    textToSay = "This is the menu page, you can pick and choose your favorite items";
                    break;
                case "Login":
                    textToSay = "This is the Login page, please fill out your information, if you don't have account with us please go to the sign up page";
                    break;
                case "Sign Up":
                    textToSay = "This is the Sign Up page, please fill out your information";
                    break;
                case "Contact":
                    textToSay = "This is the contact page please email mhoekstra5@email.davenport.edu for any questions";
                    break;
                case "View Account":
                    textToSay = "This is the view account page. Here you can change your password, email, or username.";
                    break;
                default:
                    play = false;
                    break;
            }

            if(play) this.Page.RegisterAsyncTask(new PageAsyncTask(TTS));
        }

        protected void btnAbout_Click(System.Object sender, System.EventArgs e)
        {
            Response.Redirect("About.aspx");
        }


        protected void btnMenu_Click(System.Object sender, System.EventArgs e)
        {
            Response.Redirect("MenuMeals.aspx");
        }

        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        protected void btnSignUp_Click(object sender, EventArgs e)
        {
            Response.Redirect("SignUp.aspx");
        }

        protected void Contactbtn_Click(object sender, EventArgs e)
        {
            
            this.Page.RegisterAsyncTask(new PageAsyncTask(TTS));
            Response.Redirect("Contact.aspx");
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            HttpCookie currentUserCookie = HttpContext.Current.Request.Cookies["LoginInfo"];
            HttpContext.Current.Response.Cookies.Remove("LoginInfo");
            currentUserCookie.Expires = DateTime.Now.AddDays(-10);
            currentUserCookie.Value = null;
            HttpContext.Current.Response.SetCookie(currentUserCookie);
            currentUserCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            HttpContext.Current.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
            currentUserCookie.Expires = DateTime.Now.AddDays(-10);
            currentUserCookie.Value = null;
            HttpContext.Current.Response.SetCookie(currentUserCookie);
            Request.Cookies.Clear();
            Response.Redirect("Login.aspx");
        }

        protected void btnAccount0_Click(object sender, EventArgs e)
        {
            Response.Redirect("ViewAccount.aspx");
        }

        private async Task TTS()
        {
            // you can set output file name as method argument or generated from text
            Task task = Task.Run(() =>
            {
                using (SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer())
                {
                    speechSynthesizer.SetOutputToWaveFile(Server.MapPath("/Audio/") + fileName + ".wav");
                    speechSynthesizer.Speak(textToSay);
                }
            });
            await task;
        }
    }
}