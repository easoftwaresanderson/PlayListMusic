using System;
using APIMusicPlayLists.Infra.Shared.DTOs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AppMusicPlayLists.Services.NetworkServices;
using System.Net.Http;
using Newtonsoft.Json;
using AppMusicPlayLists.Models;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace AppMusicPlayLists.Services.ApiServices
{
    public class MusicServices : IMusicServices
    {

        readonly INetworkService _networkService;

        string _apiEndPoint = "";

        public MusicServices()
        {
            _networkService = new NetworkService();
            _apiEndPoint = AppSettings.ApiBaseAddressSSL + "/api/v1/music";
        }

        public async Task<MusicDTO> GetItemAsync(int id)
        {
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"{_apiEndPoint}{id}");

                var rawResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<MusicDTO>(rawResponse);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        //public async Task<IEnumerable<MusicDTO>> GetItemsAsync(bool forceRefresh = false)
        //{
        //    try
        //    {
        //        var httpClient = new HttpClient();
        //        var response = await httpClient.GetAsync($"{_apiEndPoint}");

        //        var rawResponse = await response.Content.ReadAsStringAsync();
        //        return JsonConvert.DeserializeObject<IEnumerable<MusicDTO>>(rawResponse);

        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message);
        //        throw;
        //    }

        //}

        async Task<IEnumerable<MusicDTO>> GetRequest()
        {
            
            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, errors) => { return true; };

            var httpClient = new HttpClient(httpClientHandler);

            var response = await httpClient.GetAsync($"{_apiEndPoint}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var rawResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<MusicDTO>>(rawResponse);
           
        }

        public async Task<IEnumerable<MusicDTO>> GetItemsAsync(bool forceRefresh = false)
        {
            ObservableCollection<MusicDTO> musics = new ObservableCollection<MusicDTO>();

            try
            {
                var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //string api = AppSettings.ApiBaseAddress;

                var func = new Func<Task<IEnumerable<MusicDTO>>>(() => GetRequest());

                var items = await _networkService.Retry<IEnumerable<MusicDTO>>(func, 3, OnRetry);

                return items;
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return musics;
        }

        Task OnRetry(Exception e, int retryCount)
        {
            return Task.Factory.StartNew(() => {
                System.Diagnostics.Debug.WriteLine($"Tentativa #{retryCount}");
            });
        }


    }
}