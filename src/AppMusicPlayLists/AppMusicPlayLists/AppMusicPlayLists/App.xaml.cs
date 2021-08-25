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
        public Command ExecuteTasksCommand { get; }

        //public Command DeviceInfoCommand { get; }



        public App()
        {
            InitializeComponent();

            DependencyService.Register<MusicServices>();
            DependencyService.Register<PlayListServices>();
            DependencyService.Register<DeviceServices>();

            //ExecuteTasksCommand = new Command(async () => await ExecuteTask());
            //ExecuteTasksCommand.Execute(this);


            MainPage = new AppShell();
        }

        //private async Task ExecuteTask()
        //{


        //}

        protected override void OnStart()
        {
       
        }


        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }


    }
}
