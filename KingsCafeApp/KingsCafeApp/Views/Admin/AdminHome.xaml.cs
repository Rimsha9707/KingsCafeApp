using KingsCafeApp.Views.AWR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KingsCafeApp.Views.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminHome : ContentPage
    {
        public AdminHome()
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

        private async void btnWorker_Clicked(object sender, EventArgs e)
        {
            string selected = "Worker";
            await Navigation.PushAsync(new WorkerList(selected));
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