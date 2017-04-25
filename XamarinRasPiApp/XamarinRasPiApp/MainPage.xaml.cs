using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinRasPiApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            addressEntry.Completed += (s, e) => portEntry.Focus();
            continueButton.Clicked += ContinueAsync;
            resetButton.Clicked += Reset;
        }

        async void ContinueAsync(object sender, EventArgs e)
        {
            showLoading(true);
            string address = string.Format("http://{0}:{1}", addressEntry.Text, portEntry.Text);
            //DisplayAlert("Address", address, "OK");
            //Navigation.PushAsync(new PowerPage());
            //App.Current.MainPage = new PowerPage(address);
            //label1.Text = this.address;
            string url = address + "/check";


            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                using (
                    var response = (HttpWebResponse)(
                        await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null)
                    )
                )
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = new StreamReader(receiveStream);
                    string str = readStream.ReadToEnd();
                    dynamic obj = JsonConvert.DeserializeObject(str);

                    if (obj.error == true)
                    {
                        // Successful Login
                        //return true;
                        showLoading(false);
                        await DisplayAlert("Error", "Cannot connect.", "OK");

                    }
                    else
                    {
                        // Login Failed
                        //return false;
                        showLoading(false);

                        App.Current.MainPage = new SensorsPage(address);
                    }
                }

            }
            catch (Exception exc)
            {
                showLoading(false);
                await DisplayAlert("Error", "Cannot connect.", "OK");
            }


        }
        void Reset(object sender, EventArgs e)
        {
            addressEntry.Text = string.Empty;
            portEntry.Text = string.Empty;

        }
        void showLoading(bool status)
        {
            loginScreen.IsVisible = !status;
            loadingScreen.IsVisible = status;
        }
    }
}
