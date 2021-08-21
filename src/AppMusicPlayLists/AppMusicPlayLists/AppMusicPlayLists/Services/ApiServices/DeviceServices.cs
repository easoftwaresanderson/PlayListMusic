using APIMusicPlayLists.Infra.Shared.DTOs;
using AppMusicPlayLists.Models;
using AppMusicPlayLists.Services.ApiServices;
using AppMusicPlayLists.Services.NetworkServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AppMusicPlayLists.Services.ApiServices
{

    public class DeviceServices : IDeviceServices
    {
        public async Task<ResultDTO> AddItemAsync(DeviceDTO reg)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            var funcPost = new Func<Task<ResultDTO>>(() => PostRequest(_apiEndPoint, reg));

            var items = await _networkService.Retry<ResultDTO>(funcPost, 3, OnRetry);

            return items;


        }
        readonly INetworkService _networkService;

        string _apiEndPoint = "";

        public DeviceServices()
        {
            _networkService = new NetworkService();
            _apiEndPoint = AppSettings.ApiBaseAddressSSL + "/api/v1/device";
        }

        public async Task<DeviceDTO> GetItemAsync(int id)
        {
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"{_apiEndPoint}{id}");

                var rawResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<DeviceDTO>(rawResponse);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<DeviceDTO> GetDeviceByUniqueID(string id)
        {
            try
            {
                var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var func = new Func<Task<DeviceDTO>>(() => GetRequestItem($"{_apiEndPoint}/uniqueid/{id}"));

                var item =  await _networkService.Retry<DeviceDTO>(func, 3, OnRetry);

                return item;


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }
        async Task<IEnumerable<DeviceDTO>> GetRequest(string apiEndPoint)
        {

            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, errors) => { return true; };

            var httpClient = new HttpClient(httpClientHandler);

            var response = await httpClient.GetAsync($"{apiEndPoint}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var rawResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<DeviceDTO>>(rawResponse);

        }

        async Task<ResultDTO> PostRequest(string apiEndPoint, DeviceDTO reg)
        {

            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, errors) => { return true; };

            var httpClient = new HttpClient(httpClientHandler);

            string jsonBody = JsonConvert.SerializeObject(reg);

            var content = new System.Net.Http.StringContent(jsonBody, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = new HttpMethod("POST"),
                RequestUri = new Uri($"{apiEndPoint}"),
                Content = content
            };

            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var rawResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ResultDTO>(rawResponse);

        }

        async Task<DeviceDTO> GetRequestItem(string apiEndPoint)
        {

            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, errors) => { return true; };

            var httpClient = new HttpClient(httpClientHandler);

            var response = await httpClient.GetAsync($"{apiEndPoint}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var rawResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<DeviceDTO>(rawResponse);

        }

        public async Task<IEnumerable<DeviceDTO>> GetItemsAsync(bool forceRefresh = false)
        {
            ObservableCollection<DeviceDTO> musics = new ObservableCollection<DeviceDTO>();

            try
            {
                var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var func = new Func<Task<IEnumerable<DeviceDTO>>>(() => GetRequest(_apiEndPoint));

                var items = await _networkService.Retry<IEnumerable<DeviceDTO>>(func, 3, OnRetry);

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
            return Task.Factory.StartNew(() =>
            {
                System.Diagnostics.Debug.WriteLine($"Tentativa #{retryCount}");
            });
        }

        public Task<ResultDTO> DeleteItemAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResultDTO> UpdateItemAsync(DeviceDTO item)
        {
            throw new NotImplementedException();
        }

   
    }
}

