using KingsCafeApp.LoginSystem;
using KingsCafeApp.Models;
using KingsCafeApp.Views.Admin;
using KingsCafeApp.Views.Riders;
using KingsCafeApp.Views.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KingsCafeApp.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class loginUser : ContentPage
    {
        public loginUser()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {


                if (string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtPassword.Text))
                {
                    await DisplayAlert("Error", "Please fill required fields and try again", "Ok");
                    return;
                }
                LoadingInd.IsRunning = true;
                var check = (await App.firebaseDatabase.Child("User").OnceAsync<User>()).FirstOrDefault(x => x.Object.Email == txtEmail.Text && x.Object.Password == txtPassword.Text);
                User u = check.Object;
                if (check == null)
                {
                    
                    await DisplayAlert("Error", "Invalid Email or Password is incorrect", "ok");
                    LoadingInd.IsRunning = false;
                    return;
                }
                if (check.Object.Type=="Worker")
                {
                    App.LoggedInUser = check.Object;
                    App.Current.MainPage = new NavigationPage(new Worker_Home());
                    LoadingInd.IsRunning = false;
                }
                if (check.Object.Type == "Rider")
                {
                    App.LoggedInUser = check.Object;
                    App.Current.MainPage = new NavigationPage(new Rider_Side());
                    LoadingInd.IsRunning = false;
                }
                if (check.Object.Type == "Admin")
                {
                    App.LoggedInUser = check.Object;
                    App.Current.MainPage = new AdminSidebar();
                    LoadingInd.IsRunning = false;
                }
            }
            catch (Exception ex)
            {
                LoadingInd.IsRunning = false;
                await DisplayAlert("Error", "Something went wrong, please try again later. \nError: " + ex.Message, "Ok");

            }
        }

        
        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            btnLogin.IsVisible = false;
            btnGetPass.IsVisible = true;
            btnreset.IsVisible = true;
        }
        private void btnreset_Clicked(object sender, EventArgs e)
        {
            btnLogin.IsVisible = true;
            btnLogin.IsVisible = true;
            btnGetPass.IsVisible = false;
            btnreset.IsVisible = false;
        }

        public async void btnGetPass_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new ForgetPassword();
        }

        private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            if (txtPassword.IsPassword == true)
            {
                txtPassword.IsPassword = false;
                PsHide.IsVisible = true;
                PsShow.IsVisible = false;
            }
            else
            {
                txtPassword.IsPassword = true;
                PsHide.IsVisible = false;
                PsShow.IsVisible = true;
            }

        }

        private void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
        {
            if (txtPassword.IsPassword == true)
            {
                txtPassword.IsPassword = false;
                PsHide.IsVisible=true;
                PsShow.IsVisible = false;
            }
            else
            {
                txtPassword.IsPassword = true;
                PsHide.IsVisible = false;
                PsShow.IsVisible = true;
            }
           
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new StartPage();
        }
    }
}