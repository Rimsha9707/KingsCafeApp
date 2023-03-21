using KingsCafeApp.Login;
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

namespace KingsCafeApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            InitializeComponent();
            Animation();
        }

        //animation for logo
        public async void Animation()
        {
           
            await image.RotateTo(360, 10000);
            image.Rotation = 0;
        }


        private  void btnGetStarted_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new Views.Customer.CustomerSidebar();
            //moving from one to another without navigation
        }

        private void TapGestureRecognizer_TappedAdmin(object sender, EventArgs e)
        {
            App.Current.MainPage = new AdminSidebar();
        }
        private void TapGestureRecognizer_TappedWorker(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new Worker_Home());
        }

        private void TapGestureRecognizer_TappedRider(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new Rider_Side());
        }

    }
}