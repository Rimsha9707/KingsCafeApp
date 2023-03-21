using KingsCafeApp.Helpers;
using KingsCafeApp.LoginSystem;
using KingsCafeApp.Models;
using KingsCafeApp.Views.Admin;
using KingsCafeApp.Views.Customer;
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
    public partial class ForgetPassword : ContentPage
    {
        public ForgetPassword()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                var check = (await App.firebaseDatabase.Child("User").OnceAsync<User>()).FirstOrDefault(x => x.Object.Email == txtEmail.Text);
                if (check == null)
                {
                    LoadingInd.IsRunning = false;
                    await DisplayAlert("Alert", "Password is sent successfuly. Please Check your Email.", "ok");
                    App.Current.MainPage = new CustomerSidebar();
                }
                if (check!=null)
                {
                    EmailHelper emailHelper = new EmailHelper();
                    emailHelper.EmailSender(check.Object.Email, "Forgot Password", "Your Password = " + check.Object.Password);
                    await DisplayAlert("Error", "Email not found", "ok");
                }
            }
            catch (Exception ex)
            {
                LoadingInd.IsRunning = false;
                await DisplayAlert("Error", "Something went wrong, please try again later. \nError: " + ex.Message, "Ok");

            }

        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            App.Current.MainPage = new CustomerSidebar();
        }
    }
}