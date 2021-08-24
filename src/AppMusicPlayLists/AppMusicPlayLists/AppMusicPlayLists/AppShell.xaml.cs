using AppMusicPlayLists.Models;
using AppMusicPlayLists.ViewModels;
using AppMusicPlayLists.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppMusicPlayLists
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            GetPermissions();

           
        }
           

        private async Task GetPermissions()
        {
            bool bCheckPermissao = await CheckPermissoes();
        }

        private async Task<bool> CheckPermissoes()
        {
            try
            {
                var status = await Utils.CheckAndRequestPermissionAsync(new Permissions.StorageWrite());

                if (status != PermissionStatus.Granted)
                {
                    await DisplayAlert("Alert", "Permissão de gravação no storage negada", "OK");
                    return false;
                }

                status = await Utils.CheckAndRequestPermissionAsync(new Permissions.StorageRead());

                if (status != PermissionStatus.Granted)
                {
                    await DisplayAlert("Alert", "Permissão de leitura do storage negada.", "OK");
                    return false;
                }

                //status = await Utils.CheckAndRequestPermissionAsync(new Permissions.LocationWhenInUse());

                //if (status != PermissionStatus.Granted)
                //{
                //    await DisplayAlert("Alert", "Permissão de localização negada.", "OK");
                //    return false;
                //}

                return true;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }



    }
}
