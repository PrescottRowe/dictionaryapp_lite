using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;

namespace hw6_dictionary
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            CrossConnectivity.Current.ConnectivityChanged += HandleConnectivityChanged;
        }
        void HandleConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            Type currentPage = this.MainPage.GetType();
            if (e.IsConnected && currentPage != typeof(MainPage)) this.MainPage = new MainPage();
            else if (!e.IsConnected && currentPage != typeof(MainPage)) this.MainPage = new MainPage();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
        public string SearchWord { get; set; }
    }
}
