using AppMusicPlayLists.Models;
using AppMusicPlayLists.Services.ApiServices;
using AppMusicPlayLists.Services.LocalServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppMusicPlayLists
{
    public partial class App : Application
    {


        public App()
        {
            InitializeComponent();

            DependencyService.Register<MusicServices>();
            DependencyService.Register<PlayListServices>();
            DependencyService.Register<DeviceServices>();

            MainPage = new AppShell();
        }


        protected override void OnStart()
        {
            if (ConnectionDB.OpenConnnection())
            {
                Debug.WriteLine("Fail to open Database");
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }


    }
}
