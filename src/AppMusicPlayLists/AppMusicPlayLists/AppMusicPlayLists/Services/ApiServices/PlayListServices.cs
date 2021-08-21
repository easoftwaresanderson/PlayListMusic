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
using System.Text;
using APIMusicPlayLists.Infra.Shared.Commands;

namespace AppMusicPlayLists.Services.ApiServices
{
    public class PlayListServices : IPlayListServices
    {

        readonly INetworkService _networkService;

        string _apiEndPoint = "";

        public PlayListServices()
        {
            _networkService = new NetworkService();
            _apiEndPoint = AppSettings.ApiBaseAddressSSL + "/api/v1/playlist";
        }

        public async Task<ResultDTO> AddItemAsync(PlayListDTO reg)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var funcPost = new Func<Task<ResultDTO>>(() => PostRequest(_apiEndPoint, reg));

            //var item =  await _networkService.Retry<IEnumerable<ResultDTO>(funcPost, 3, OnRetry);

            var items = await _networkService.Retry<ResultDTO>(funcPost, 3, OnRetry);

            return items;
        }

        public async Task<ResultDTO> FavoriteSong(PlayListFavoriteCommand command)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var funcPost = new Func<Task<ResultDTO>>(() => FavoriteRequest($"{_apiEndPoint}/favoritesong/", command));

            var items = await _networkService.Retry<ResultDTO>(funcPost, 3, OnRetry);

            return items;
        }

        public async Task<PlayListDTO> GetItemAsync(int id)
        {
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"{_apiEndPoint}{id}");

                var rawResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PlayListDTO>(rawResponse);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }        
        
        public async Task<PlayListDTO> GetPlayListByDeviceIDAsync(int id)
        {
            try
            {
                var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var func = new Func<Task<PlayListDTO>>(() => GetRequestItem($"{_apiEndPoint}/device/{id}"));

                var item = await _networkService.Retry<PlayListDTO>(func, 3, OnRetry);

                return item;


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        } 

        async Task<IEnumerable<PlayListDTO>> GetRequest(string apiEndPoint)
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

            return JsonConvert.DeserializeObject<IEnumerable<PlayListDTO>>(rawResponse);

        }          
        
        async Task<ResultDTO> PostRequest(string apiEndPoint, PlayListDTO reg)
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

        async Task<ResultDTO> FavoriteRequest(string apiEndPoint, PlayListFavoriteCommand reg)
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


        async Task<PlayListDTO> GetRequestItem(string apiEndPoint)
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

            return JsonConvert.DeserializeObject<PlayListDTO>(rawResponse);

        }

        public async Task<IEnumerable<PlayListDTO>> GetItemsAsync(bool forceRefresh = false)
        {
            ObservableCollection<PlayListDTO> musics = new ObservableCollection<PlayListDTO>();

            try
            {
                var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //string api = AppSettings.ApiBaseAddress;

                var func = new Func<Task<IEnumerable<PlayListDTO>>>(() => GetRequest(_apiEndPoint));

                var items = await _networkService.Retry<IEnumerable<PlayListDTO>>(func, 3, OnRetry);

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

        public Task<ResultDTO> DeleteItemAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResultDTO> UpdateItemAsync(PlayListDTO item)
        {
            throw new NotImplementedException();
        }
    }
}
