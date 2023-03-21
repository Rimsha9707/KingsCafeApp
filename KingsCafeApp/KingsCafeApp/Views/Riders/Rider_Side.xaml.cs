using Firebase.Database.Query;
using KingsCafeApp.Models;
using KingsCafeApp.Views.Admin;
using KingsCafeApp.Views.AWR;
using KingsCafeApp.Views.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KingsCafeApp.Views.Riders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Rider_Side : TabbedPage
    {
     
        public Rider_Side()
        {
            InitializeComponent();
            LoadingInd.IsRunning = true;
            LoadingInd2.IsRunning = true;

            LoadData();
            LoadDataDelivered();

            LoadingInd.IsRunning = false;
            LoadingInd2.IsRunning = false;


        }

        async void LoadData()
        {
            try
            {
                DataList.ItemsSource = (await App.firebaseDatabase.Child("Order").OnceAsync<Order>()).Select(x => new Order
                {
                    Address = x.Object.Address,
                    DeliveryDate = x.Object.DeliveryDate,
                    Email = x.Object.Email,
                    OrderDate = x.Object.OrderDate,
                    OrderID = x.Object.OrderID,
                    OrderTime = x.Object.OrderTime,
                    PaymentMethod = x.Object.PaymentMethod,
                    Phone = x.Object.Phone,
                    Name = x.Object.Name,
                    Status = x.Object.Status,
                    AssignedRider = x.Object.AssignedRider,
                    AddressType = x.Object.AddressType,
                    Latitude = x.Object.Latitude,
                    Logitude = x.Object.Logitude
                }).Where(x => x.Status != "Delivered").ToList();
                var item = DataList;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Something went wrong, please try again later. \nError: " + ex.Message, "Ok");
            }

        }
        async void LoadDataDelivered()
        {
            try
            {
                DataListDelivered.ItemsSource = (await App.firebaseDatabase.Child("Order").OnceAsync<Order>()).Select(x => new Order
                {
                    Address = x.Object.Address,
                    DeliveryDate = x.Object.DeliveryDate,
                    Email = x.Object.Email,
                    OrderDate = x.Object.OrderDate,
                    OrderID = x.Object.OrderID,
                    OrderTime = x.Object.OrderTime,
                    PaymentMethod = x.Object.PaymentMethod,
                    Phone = x.Object.Phone,
                    Name = x.Object.Name,
                    Status = x.Object.Status,
                    AssignedRider = x.Object.AssignedRider,
                    AddressType = x.Object.AddressType,
                    Latitude = x.Object.Latitude,
                    Logitude = x.Object.Logitude
                }).Where(x=>x.Status == "Delivered").ToList();
                var item = DataList;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Something went wrong, please try again later. \nError: " + ex.Message, "Ok");
            }

        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new StartPage();
        }
        
        private async void DataList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var selected = e.Item as Order;

                LoadingInd.IsRunning = true;
                LoadingInd2.IsRunning = true;

                var item = (await App.firebaseDatabase.Child("Order").OnceAsync<Order>()).FirstOrDefault(a => a.Object.OrderID == selected.OrderID);
                var choice = await DisplayActionSheet("Process Order", "Close", "", "View Details", "Delivered", "Cancelled");

                if (choice == "View Details")
                {
                    await Navigation.PushAsync(new Order_Detail(item.Object));
                }
                if (choice == "Pending")
                {
                    item.Object.Status = "Pending";
                    await App.firebaseDatabase.Child("Order").Child(item.Key).PutAsync(item.Object);
                    await DisplayAlert("Message", item.Object.OrderID + "'s" + " order is Delivered now.", "Ok");
                }
                if (choice == "Ready")
                {
                    item.Object.Status = "Ready";
                    await App.firebaseDatabase.Child("Order").Child(item.Key).PutAsync(item.Object);
                    await DisplayAlert("Message", item.Object.OrderID + "'s" + " order is Delivered now.", "Ok");
                }
                if (choice == "Delivered")
                {
                    item.Object.Status = "Delivered";
                    await App.firebaseDatabase.Child("Order").Child(item.Key).PutAsync(item.Object);
                    await DisplayAlert("Message", item.Object.OrderID + "'s" + " order is Delivered now.", "Ok");
                }
                if (choice == "Cancelled")
                {
                    item.Object.Status = "Cancelled";
                    await App.firebaseDatabase.Child("Order").Child(item.Key).PutAsync(item.Object);
                    await DisplayAlert("Message", item.Object.OrderID + "'s" + " order is Delivered now.", "Ok");
                }
                LoadData();
                LoadDataDelivered();

                LoadingInd.IsRunning = false;
                LoadingInd2.IsRunning = false;


            }

            catch (Exception ex)
            {


                await DisplayAlert("Error", "Something went wrong, please try again later. \nError: " + ex.Message, "Ok");
            }
        }

        private async void DataList_ItemTappedDelivered(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var selected = e.Item as Order;

                LoadingInd.IsRunning = true;
                LoadingInd2.IsRunning = true;

                var item = (await App.firebaseDatabase.Child("Order").OnceAsync<Order>()).FirstOrDefault(a => a.Object.OrderID == selected.OrderID);
                var choice = await DisplayActionSheet("Process Order", "Close", "", "View Details", "Delivered", "Cancelled");

                if (choice == "View Details")
                {
                    await Navigation.PushAsync(new Order_Detail(item.Object));
                }
                if (choice == "Pending")
                {
                    item.Object.Status = "Pending";
                    await App.firebaseDatabase.Child("Order").Child(item.Key).PutAsync(item.Object);
                    await DisplayAlert("Message", item.Object.OrderID + "'s" + " order is Delivered now.", "Ok");
                }
                if (choice == "Ready")
                {
                    item.Object.Status = "Ready";
                    await App.firebaseDatabase.Child("Order").Child(item.Key).PutAsync(item.Object);
                    await DisplayAlert("Message", item.Object.OrderID + "'s" + " order is Delivered now.", "Ok");
                }
                if (choice == "Delivered")
                {
                    item.Object.Status = "Delivered";
                    await App.firebaseDatabase.Child("Order").Child(item.Key).PutAsync(item.Object);
                    await DisplayAlert("Message", item.Object.OrderID + "'s" + " order is Delivered now.", "Ok");
                }
                if (choice == "Cancelled")
                {
                    item.Object.Status = "Cancelled";
                    await App.firebaseDatabase.Child("Order").Child(item.Key).PutAsync(item.Object);
                    await DisplayAlert("Message", item.Object.OrderID + "'s" + " order is Delivered now.", "Ok");
                }
                LoadData();
                LoadDataDelivered();

                LoadingInd.IsRunning = false;
                LoadingInd2.IsRunning = false;
            }

            catch (Exception ex)
            {


                await DisplayAlert("Error", "Something went wrong, please try again later. \nError: " + ex.Message, "Ok");
            }
        }
        private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new User_Profile(App.LoggedInUser));
        }
    }
}