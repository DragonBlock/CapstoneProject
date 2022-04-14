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
        string fileName = "home";
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

            LoadNarration(this.Page.Title);
        }

        public void LoadNarration(string pageTitle)
        {
            bool play = true;
            switch (pageTitle)
            {
                case "Home Page":
                    textToSay = "Welcome to the home page!";
                    fileName = "home";
                    break;
                case "MenuMeals":
                    textToSay = "This is the menu page, here you can pick and choose your favorite items to order";
                    fileName = "menu";
                    break;
                case "Login":
                    textToSay = "This is the Login page, please fill out your information, if you don't have account with us please go to the sign up page";
                    fileName = "login";
                    break;
                case "Sign Up":
                    textToSay = "This is the Sign Up page, please fill out your information";
                    fileName = "signup";
                    break;
                case "Contact":
                    textToSay = "This is the contact page please email mhoekstra5@email.davenport.edu for any questions";
                    fileName = "contact";
                    break;
                case "View Account":
                    textToSay = "This is the view account page. Here you can change your password, email, or username.";
                    fileName = "view";
                    break;
                case "See Cart":
                    textToSay = "This is the see cart page, here you can see what items are in your cart.";
                    fileName = "cart";
                    break;
                case "Checkout":
                    textToSay = "This is the checkout page, here you can see your final order and purchase it.";
                    fileName = "check";
                    break;
                case "About":
                    textToSay = "VirtU Food Delivery is an aspiring online food ordering service that can deliver"+
                        " a variety of food straight to your door! VirtU Food Delivery was founded in 2022.";
                    fileName = "check";
                    break;
                default:
                    play = false;
                    break;
            }

            if (play)
            {
                this.Page.RegisterAsyncTask(new PageAsyncTask(TTS));
            }
        }


        protected void btnAbout_Click(System.Object sender, System.EventArgs e)
        {
            LoadNarration("About");
            Response.Redirect("About.aspx");
        }


        protected void btnMenu_Click(System.Object sender, System.EventArgs e)
        {
            LoadNarration("MenuMeals");
            Response.Redirect("MenuMeals.aspx");
        }

        protected void btnHome_Click(object sender, EventArgs e)
        {
            LoadNarration("Home Page");
            Response.Redirect("Default.aspx");
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            LoadNarration("Login");
            Response.Redirect("Login.aspx");
        }

        protected void btnSignUp_Click(object sender, EventArgs e)
        {
            LoadNarration("Sign Up");
            Response.Redirect("SignUp.aspx");
        }

        protected void Contactbtn_Click(object sender, EventArgs e)
        {
            LoadNarration("Contact");
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
            LoadNarration("Login");
            Response.Redirect("Login.aspx");
        }

        protected void btnAccount0_Click(object sender, EventArgs e)
        {
            LoadNarration("View Account");
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
            File.Delete(Server.MapPath("/Audio/") + "speech.wav");
            File.Copy(Server.MapPath("/Audio/") + fileName + ".wav", Server.MapPath("/Audio/") + "speech.wav");
            audioSource.Src = "/Audio/" + fileName + ".wav";
        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            LoadNarration("Checkout");
            Response.Redirect("Checkout.aspx");
        }

        protected void btnCart_Click(object sender, EventArgs e)
        {
            LoadNarration("See Cart");
            Response.Redirect("SeeCart.aspx");
        }
    }
}