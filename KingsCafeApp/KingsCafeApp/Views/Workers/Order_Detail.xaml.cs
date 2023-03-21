using KingsCafeApp.Models;
using KingsCafeApp.ViewModels;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;

namespace KingsCafeApp.Views.Workers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Order_Detail : ContentPage
    {
        public Order_Detail(Order o)
        {
            InitializeComponent();
            txtUserName.Text = o.Name;
            txtUserAddress.Text = o.Address;
            txtUserEmail.Text = o.Email;
            txtUserPhone.Text = o.Phone;
            TxtLocationType.Text = o.AddressType;
            LoadingInd.IsRunning = false;

            UpdateCartAsync(o.OrderID);

            if (o.AddressType == "Map")
            {
                MapView.IsVisible = true;
                GetLocationMap(o.Logitude,o.Latitude);


            }
        }

        async void UpdateCartAsync(int id)
        {

            var orderdetails = (await App.firebaseDatabase.Child("OrderDetail").OnceAsync<Models.OrderDetail>()).Where(x=>x.Object.OrderFID == id).ToList();


            List<imageCell_VM> CartItems = new List<imageCell_VM>();
            decimal? Amount = 0;
            foreach (var item in orderdetails)
            {
                var prod = (await App.firebaseDatabase.Child("FoodItem").OnceAsync<Models.FoodItem>()).FirstOrDefault(x => x.Object.ItemID == item.Object.ItemFID);

                decimal? total = (decimal?)(prod.Object.SalePrice * (item.Object.Quantity));
                Amount += total;

                CartItems.Add(new imageCell_VM
                {
                    ID = prod.Object.ItemID,
                    image = prod.Object.Image,
                    Name = prod.Object.Name,
                    Detail = "Rs. " + prod.Object.SalePrice + " X  " + item.Object.Quantity + " = Total Rs. " + total.ToString()
                });
            }

            App.Total = Amount;

            lblTotal.Text = Amount.ToString();
            DataList.ItemsSource = CartItems;

        }

        public async void GetLocationMap(double longi, double lat)
        {
            try
            {
                LoadingInd.IsRunning = true;

                await DisplayAlert("Message", "GPS and Internet will be used to access your current location. Please confirm that you have enabled the GPS and Internet.", "Ok");
                MapView.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(lat, longi), Distance.FromMiles(1)));
                var pos = new Xamarin.Forms.Maps.Position(lat, longi);
                MapView.Pins.Add(new Pin { Label = "Your Location", Position = pos });
                LoadingInd.IsRunning = false;



            }
            catch (Exception ex)
            {
                await DisplayAlert("Message", "Somthing went wrong. This may be a problem with internet or application. Please ensure that you have a working internet connection and GPS enabled. \nError details : " + ex.Message, "Ok");
            }
        }
    }
}