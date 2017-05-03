using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinRasPiApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SensorsPage : ContentPage
    {
        string address = "";
        public SensorsPage()
        {
            InitializeComponent();

            //For Development pupose only
            this.address = "http://raspi.azurewebsites.net/api";
            InitializeButtonEvents();
            GetInitialValuesAsync();
        }
        public SensorsPage(string address)
        {
            InitializeComponent();
            this.address = address;
            showTempLoading(false);
            //showUltLoading(false);
            InitializeButtonEvents();
            GetInitialValuesAsync();
        }
        private void GetInitialValuesAsync()
        {
            GetTemperatureDataAsync(null, null);
            // GetUltrasonicDataAsync(null, null);
        }
        private void InitializeButtonEvents()
        {
            getTempButton.Clicked += GetTemperatureDataAsync;
            // getUltrasonicButton.Clicked += GetUltrasonicDataAsync;
            // disconectButton.Clicked += Disconnect;
        }
        private void showTempLoading(bool status)
        {
            temperatureValue.IsVisible = temperatureTimeStamp.IsVisible = !status;
            loadingTempIndicator.IsVisible = loadingTempTimeIndicator.IsVisible = status;
            
        }
        private void showUltLoading(bool status)
        {
            // ultrasonicValue.IsVisible = !status;
            // loadingUltIndicator.IsVisible = status;
        }
        private async void GetTemperatureDataAsync(object sender, EventArgs e)
        {
            showTempLoading(true);
            string url = address + "/temperature";

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

                    if (obj.error != true)
                    {
                        showTempLoading(false);
                        temperatureValue.Text = Convert.ToDouble(obj.data.temperature.ToString()).ToString() + " °C";
                        string timestamp = obj.data.timestamp.ToString();
                        string date = timestamp.Split(' ')[0];
                        string time = timestamp.Split(' ')[1].Substring(0, 8);
                        temperatureTimeStamp.Text = time + " " + date;
                    }
                    else
                    {
                        showTempLoading(false);
                        temperatureValue.Text = "Error.";
                        temperatureTimeStamp.Text = "Error.";
                        await DisplayAlert("Error", "Error getting temperature value.", "OK");
                    }


                }

            }
            catch (Exception exc)
            {
                showTempLoading(false);
                temperatureValue.Text = "Error.";
                temperatureTimeStamp.Text = "Error.";
                await DisplayAlert("Error", "Error getting temperature value.", "OK");
            }
        }
        /*private async void GetUltrasonicDataAsync(object sender, EventArgs e)
        {
            showUltLoading(true);
            string url = address + "/ultrasonic";

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

                    if (obj.value != null)
                    {
                        showUltLoading(false);
                        ultrasonicValue.Text = Convert.ToDouble(obj.value.ToString()).ToString() + " cm";
                    }
                    else
                    {
                        showUltLoading(false);
                        await DisplayAlert("Error", "Error getting ultrasonic value.", "OK");
                    }
                }

            }
            catch (Exception exc)
            {
                showUltLoading(false);
                await DisplayAlert("Error", "Error getting ultrasonic value.", "OK");
            }
        }*/
        private void Disconnect(object sender, EventArgs e)
        {
            App.Current.MainPage = new MainPage();
        }
    }
}
