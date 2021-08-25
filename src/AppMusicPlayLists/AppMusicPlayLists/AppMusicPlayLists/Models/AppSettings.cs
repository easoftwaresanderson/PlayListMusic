using APIMusicPlayLists.Infra.Shared.DTOs;
using AppMusicPlayLists.Services.ApiServices;
using Plugin.DeviceInfo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppMusicPlayLists.Models
{
    public static class AppSettings
    {
        private static IDeviceServices DeviceData => DependencyService.Get<IDeviceServices>();

        //public static string IPAddress = DeviceInfo.Platform == DevicePlatform.Android ? "10.0.2.2" : "localhost";
        public static string IPAddress = "192.168.1.100";
        public static string ApiBaseAddress = $"http://{IPAddress}:25106";
        public static string ApiBaseAddressSSL = $"https://{IPAddress}:44338";

        public static DeviceDTO Device;

        public static  PlayList PlayList;
        public static  ObservableCollection<PlayListMusics> PlayListMusics;

        public static async Task GetDeviceInfo()
        {
            try
            {
                AppSettings.Device = new DeviceDTO();

                AppSettings.Device.UniqueID = CrossDeviceInfo.Current.Id;

                var device = await DeviceData.GetDeviceByUniqueID(AppSettings.Device.UniqueID);

                if (device == null)
                {
                    AppSettings.Device.Model = DeviceInfo.Model;
                    AppSettings.Device.Manufacturer = DeviceInfo.Manufacturer;
                    AppSettings.Device.Name = DeviceInfo.Name;
                    AppSettings.Device.VersionString = DeviceInfo.VersionString;
                    AppSettings.Device.Platform = DeviceInfo.Platform.ToString();
                    AppSettings.Device.Idiom = DeviceInfo.Idiom.ToString();
                    AppSettings.Device.DeviceType = DeviceInfo.DeviceType.ToString();

                    var res = await DeviceData.AddItemAsync(AppSettings.Device);

                    if (!res.Success)
                    {
                        return;
                    }

                    device = await DeviceData.GetDeviceByUniqueID(AppSettings.Device.UniqueID);

                    if (device != null)
                        AppSettings.Device.Id = device.Id;

                }
                else
                {
                    AppSettings.Device = device;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get Device info \n Exception : " + ex);
            }
        }
    }
}
