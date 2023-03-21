using Plugin.Geolocator;
using KingsCafeApp.Models;
using Plugin.Geolocator.Abstractions;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Plugin.Geolocator.Abstractions;
using Firebase.Database.Query;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using KingsCafeApp.Helpers;

namespace KingsCafeApp.Views.Customer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfirmOrder : ContentPage
    {
        public static Xamarin.Forms.Maps.Position Pos = new Xamarin.Forms.Maps.Position();

        public string AddressType = "Address";

        public ConfirmOrder()
        {
            InitializeComponent();

        }

        public async void GetLocationMap()
        {
            try
            {
                await DisplayAlert("Message", "GPS and Internet will be used to access your current location. Please confirm that you have enabled the GPS and Internet.", "Ok");

                var locator = CrossGeolocator.Current;
                var position = await locator.GetPositionAsync();

                Xamarin.Forms.Maps.Position TempPos = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude);
                Pos = TempPos;
                MapView.Pins.Add(new Pin { Label = "Your Location", Position = Pos });

                MapView.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude), Distance.FromMiles(1)));


            }
            catch (Exception ex)
            {
                await DisplayAlert("Message", "Somthing went wrong. This may be a problem with internet or application. Please ensure that you have a working internet connection and GPS enabled. \nError details : " + ex.Message, "Ok");
            }
        }

        private async void btnConfirm_Clicked(object sender, EventArgs e)
        {
            try
            {
            LoadingInd.IsRunning = true;
            int LastID, NewID = 1;

            var LastRecord = (await App.firebaseDatabase.Child("Order").OnceAsync<Order>()).FirstOrDefault();
            if (LastRecord != null)
            {
                LastID = (await App.firebaseDatabase.Child("Order").OnceAsync<Order>()).Max(a => a.Object.OrderID);
                NewID = ++LastID;
            }



            Order order = new Order()
            {
                OrderID = NewID,
                Name = txtName.Text,
                Email = txtEmail.Text,
                Phone = txtPhone.Text,
                Address = txtAddress.Text,
                OrderDate = DateTime.Now.Date,
                OrderTime = DateTime.Now.TimeOfDay,
                Status = "Pending",
                AssignedRider = "N/A",
                AddressType = AddressType,
                Logitude = Pos.Longitude,
                Latitude = Pos.Latitude,
            };

            await App.firebaseDatabase.Child("Order").PostAsync(order);

            int LastID2, NewID2 = 1;
            var lastrecord2 = (await App.firebaseDatabase.Child("OrderDetail").OnceAsync<OrderDetail>()).FirstOrDefault();
            if (lastrecord2 != null)
            {
                LastID2 = (await App.firebaseDatabase.Child("OrderDetail").OnceAsync<OrderDetail>()).Max(a => a.Object.DetailsID);
                NewID2 = ++LastID2;
            }

            double total = 0;
            
            for (int i = 0; i < App.Cart.Count; i++)
            {
                var prod = (await App.firebaseDatabase.Child("FoodItem").OnceAsync<Models.FoodItem>()).FirstOrDefault(x => x.Object.ItemID == App.Cart[i].ItemFID);

                total = total + (App.Cart[i].Quantity * prod.Object.SalePrice);
                OrderDetail detail = new OrderDetail()
                {
                    OrderFID = NewID,
                    ItemFID = App.Cart[i].ItemFID,
                    Quantity = App.Cart[i].Quantity
                };

                await App.firebaseDatabase.Child("OrderDetail").PostAsync(detail);
                    //<========================sms sending code ===================>
                   String api = "https://lifetimesms.com/json?api_token=642b689f1bbed6c4c3775914ee2b1912ebaf608169&api_secret=KingsCafe&to" + order.Phone + "7from=King's Cafe&message=Your Order is Booked Succesfully. Thanks for shopping here\n\n Regards King's Cafe";
                    WebRequest request = WebRequest.Create(api);
                    WebResponse response = request.GetResponse();
                }

            //await DisplayAlert("Success", "OrderSaved Successfully", "OK");
            LoadingInd.IsRunning = false;
        
        //==========================navigate to order success page====================================
            App.Current.MainPage = new NavigationPage(new Views.Customer.OrderConfirmation(NewID));

            }
            catch (Exception ex)
            {
                await DisplayAlert("message", "somthing went wrong. this may be a problem with internet or application. please ensure that you have a working internet connection and gps enabled. \nerror details : " + ex.Message, "ok");
            }
        }

        //===========================for validations=================================================
        private void txtPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length < 11 || e.NewTextValue.Length > 13)
            {
                lblPhoneValidation.IsVisible = true;
                lblPhoneValidation.Text = "InValid Phone !! Missing digit(s) ";
                lblPhoneValidation.TextColor = Color.Red;
            }
            else
            {
                lblPhoneValidation.Text = "valid phone no.";
                lblPhoneValidation.TextColor = Color.Green;
            }
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            var EmailPattern = @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$";
            //@ sign used to read special characters as a srting

            if (Regex.IsMatch(e.NewTextValue, EmailPattern))
            {
                lblEmailValidation.IsVisible = true;
                lblEmailValidation.Text = "Valid Email";
                lblEmailValidation.TextColor = Color.Green;
            }

            else
            {
                lblEmailValidation.Text = "InValid Email. Email must contain @ and .com ";
                lblEmailValidation.TextColor = Color.Red;
            }
        }

        private void rdAddress_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (rdAddress.IsChecked == true)
            {
                txtAddress.IsVisible= true;
                MapView.IsVisible= false;
                btnGetLocation.IsVisible = false;

                AddressType = "Address";
            }
            else
            {
                txtAddress.IsVisible = false;
                MapView.IsVisible = true;
                btnGetLocation.IsVisible = true;
                AddressType = "Map";
            }
        }

       

        private void btnGetLocation_Clicked(object sender, EventArgs e)
        {
            GetLocationMap();
        }
    }

}