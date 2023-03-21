using Firebase.Database.Query;
using KingsCafeApp.Models;
using KingsCafeApp.Views.Admin;
using KingsCafeApp.Views.AWR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KingsCafeApp.Views.Workers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Worker_Home : ContentPage
    {
        public Worker_Home()
        {
            InitializeComponent();

        }
       
       
        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new StartPage();
        }

        private async void btnProfile_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new User_Profile(App.LoggedInUser));
        }

        private async void btnRider_Clicked(object sender, EventArgs e)
        {
            string selected = "Rider";
            await Navigation.PushAsync(new WorkerList(selected));
        }

        private async void btOrder_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Manage_Orders());
        }
    }
}