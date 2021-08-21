using APIMusicPlayLists.Infra.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace AppMusicPlayLists.Models
{
    public static class AppSettings
    {
        //public static string IPAddress = DeviceInfo.Platform == DevicePlatform.Android ? "10.0.2.2" : "localhost";
        public static string IPAddress = "192.168.1.100";
        public static string ApiBaseAddress = $"http://{IPAddress}:25106";
        public static string ApiBaseAddressSSL = $"https://{IPAddress}:44338";

        public static DeviceDTO Device;

        public static  PlayListDTO PlayList;
    }
}
