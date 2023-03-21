using Firebase.Database.Query;
using KingsCafeApp.Models;
using KingsCafeApp.Views.Admin;
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
    public partial class WorkerList : ContentPage
    {
        public string S;
        public WorkerList(String Selected)
        {
             S = Selected;
            InitializeComponent();
                LoadingInd.IsRunning = true;
                LoadData(S);
                LoadingInd.IsRunning = false;           
        }

        async void LoadData(String W)
        {
            try
            {
                DataList.ItemsSource = (await App.firebaseDatabase.Child("User").OnceAsync<User>()).Select(x => new User
                {
                    UserID = x.Object.UserID,
                    Name = x.Object.Name,
                    Email = x.Object.Email,
                    Password = x.Object.Password,
                    Type = x.Object.Type,
                    Status = x.Object.Status,
                    Image = x.Object.Image
                }).ToList().Where(x => x.Type==W);
            }
            catch (Exception ex)
            {

                LoadingInd.IsRunning = false;
                await DisplayAlert("Error", "Something went wrong, please try again later. \nError: " + ex.Message, "Ok");
            }

        }

            private async void DataList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {

                var selected = e.Item as User;
                var item = (await App.firebaseDatabase.Child("User").OnceAsync<User>()).FirstOrDefault(a => a.Object.UserID == selected.UserID);
                var choice = await DisplayActionSheet("Options", "Close", "Delete", "View", "Edit");

                if (choice == "View")
                {
                    await Navigation.PushAsync(new User_Details(selected));
                }
                if (choice == "Delete")
                {
                    var q = await DisplayAlert("Confirmation", "Are you sure you want to delete " + item.Object.Name + "?", "Yes", "No");
                    if (q)
                    {
                        await App.firebaseDatabase.Child("User").Child(item.Key).DeleteAsync();
                        LoadData(S);
                        await DisplayAlert("Message", item.Object.Name + "'s" + " User is deleted permanently.", "Ok");
                    }

                }
                if (choice == "Edit")
                {
                    await Navigation.PushAsync(new Edit_User(selected));

                }

            }

            catch (Exception ex)
            {

                LoadingInd.IsRunning = false;
                await DisplayAlert("Error", "Something went wrong, please try again later. \nError: " + ex.Message, "Ok");
            }

        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new StartPage();
        }
    }
}