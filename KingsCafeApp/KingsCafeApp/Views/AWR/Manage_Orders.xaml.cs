using Firebase.Database.Query;
using KingsCafeApp.Models;
using KingsCafeApp.Views.Admin;
using KingsCafeApp.Views.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KingsCafeApp.Views.AWR
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Manage_Orders : ContentPage
    {
        public Manage_Orders()
        {
            InitializeComponent();
            LoadingInd.IsRunning = true;
            LoadData();
            LoadingInd.IsRunning = false;
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

                }).ToList();
                var item = DataList;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Something went wrong, please try again later. \nError: " + ex.Message, "Ok");
            }

        }

        async void LoadData(string S)
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

                }).ToList().Where(x=>x.Status==S);
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
                try
                {
                    var selected = e.Item as Order;
                    LoadingInd.IsRunning = true;

                    var item = (await App.firebaseDatabase.Child("Order").OnceAsync<Order>()).FirstOrDefault(a => a.Object.OrderID == selected.OrderID);
                    var choice = await DisplayActionSheet("Process Order", "Close", "", "View Details", "Assign Rider", "Pending", "Ready", "Delivered", "Cancelled");

                    if (choice == "Assign Rider")
                    {
                        await Navigation.PushAsync(new RiderList(item.Object));
                    }
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
                    LoadingInd.IsRunning = false;
                }

                catch (Exception ex)
                {

                    LoadingInd.IsRunning = false;
                    await DisplayAlert("Error", "Something went wrong, please try again later. \nError: " + ex.Message, "Ok");
                }
            }

            catch (Exception ex)
            {
                await DisplayAlert("Error", "Something went wrong, please try again later. \nError: " + ex.Message, "Ok");
            }

        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                string c;
                var choice = await DisplayActionSheet("Options", "Close", "", "Ready Orders", "Delivered Orders","Cancelled Orders", "Pending Orders");

                if (choice == "Pending Orders")
                {
                    c = "Pending";
                    LoadData(c);
                }
                if (choice == "Ready Orders")
                {
                    c = "Ready";
                    LoadData(c);

                }
                if (choice == "Delivered Orders")
                {
                    c = "Delivered";
                    LoadData(c);
                }
                if (choice == "Cancelled Orders")
                {
                    c = "Cancelled";
                    LoadData(c);
                }
                else
                    LoadData();

            }

            catch (Exception ex)
            {

                LoadingInd.IsRunning = false;
                await DisplayAlert("Error", "Something went wrong, please try again later. \nError: " + ex.Message, "Ok");
            }
        }
    }
}