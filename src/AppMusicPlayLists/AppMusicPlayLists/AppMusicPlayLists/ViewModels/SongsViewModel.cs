using APIMusicPlayLists.Infra.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;
using AppMusicPlayLists.Services.ApiServices;
using AppMusicPlayLists.Models;
using Plugin.DeviceInfo;
using Xamarin.Essentials;
using APIMusicPlayLists.Infra.Shared.Commands;

namespace AppMusicPlayLists.ViewModels
{
    public class SongsViewModel : BaseViewModel
    {
        private string _sConnected;
        private bool _bNotConnected;
        private IMusicServices MusicsData => DependencyService.Get<IMusicServices>();
        private IPlayListServices PlayListsData => DependencyService.Get<IPlayListServices>();
        private IDeviceServices DeviceData => DependencyService.Get<IDeviceServices>();


        private ObservableCollection<MusicDTO> _Musics;

        public Command LoadItemsCommand { get; }
        public Command LoadPlayListCommand { get; }
        public Command FavoriteCommand
        {
            get;
            set;
        }

        public SongsViewModel()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

            _bNotConnected = !Utils.IsInternetAvaliable();
            IsNotConnected = _bNotConnected;

            _Musics = new ObservableCollection<MusicDTO>();

            FavoriteCommand = new Command<MusicDTO>(OnFavoriteClicked);

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            //RefreshView already start execute
            LoadItemsCommand.Execute(this);

            LoadPlayListCommand = new Command(async () => await LoadPlayListExecute());
            LoadPlayListCommand.Execute(this);

          

        }

        private async void OnFavoriteClicked(MusicDTO music)
        {
            if (music != null)
            {
                int idx = _Musics.IndexOf(music);

                if (idx >= 0)
                {

                    if (AppSettings.PlayList.Musics == null)
                    {
                        AppSettings.PlayList.Musics = new List<MusicDTO>();
                    }

                    PlayListFavoriteCommand command = new PlayListFavoriteCommand();
                    command.Favorite = music.Favorite == 0 ? 1 : 0;
                    command.MusicId = music.Id;
                    command.PlayListId = AppSettings.PlayList.Id;

                    var res = await PlayListsData.FavoriteSong(command);

                    if (!res.Success)
                    {
                        return;
                    }

                    music.Favorite = command.Favorite;

                    //music.Favorited = music.Favorited == 0 ? 1 : 0;

                    _Musics[idx] = music;


                    if (music.Favorite == 0)
                    {
                        if (AppSettings.PlayList.Musics.Contains(music))
                        {
                            AppSettings.PlayList.Musics.Remove(music);
                        }
                    }
                    else
                    {
                        if (!AppSettings.PlayList.Musics.Contains(music))
                        {
                            AppSettings.PlayList.Musics.Add(music);
                        }
                    }

                }
            }
        }


        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
               
                if (_bNotConnected)
                {
                    return;
                }


                _Musics.Clear();
                GetSongs = _Musics;

                //Desabilitar para acessar a API
                var items = await MusicsData.GetItemsAsync(true);
                foreach (var item in items)
                {
                    _Musics.Add(item);
                }

                GetSongs = _Musics;

                //Mock para design da page.
                //_Musics = new ObservableCollection<MusicDTO>()
                //{
                //    new MusicDTO { Id = 1 , AlbumName = "Ten", ArtistName = "Pearl Jam" , MusicName = "Once" , AlbumImage= "pearljam.jpg" , Favorited = 0  },
                //    new MusicDTO { Id = 2 , AlbumName = "Ten", ArtistName = "Pearl Jam" , MusicName = "Alive", AlbumImage= "pearljam.jpg" ,Favorited = 0  },
                //    new MusicDTO { Id = 3 , AlbumName = "Ten", ArtistName = "Pearl Jam" , MusicName = "Even Flow", AlbumImage= "pearljam.jpg",Favorited = 0  },
                //    new MusicDTO { Id = 4 , AlbumName = "Ten", ArtistName = "Pearl Jam" , MusicName = "Black" , AlbumImage= "pearljam.jpg",Favorited = 0  },
                //    new MusicDTO { Id = 5 , AlbumName = "Ten", ArtistName = "Pearl Jam" , MusicName = "Come Back", AlbumImage= "pearljam.jpg",Favorited = 1  },
                //    new MusicDTO { Id = 6 , AlbumName = "This is U2", ArtistName = "U2" , MusicName = "One", AlbumImage= "thisisu2.png",Favorited = 1  },
                //    new MusicDTO { Id = 7 , AlbumName = "This is U2", ArtistName = "U2" , MusicName = "With or Without you", AlbumImage= "thisisu2.png", Favorited = 0 },
                //    new MusicDTO { Id = 8 , AlbumName = "U2 POP", ArtistName = "U2" , MusicName = "If God Will Send His Angels" , AlbumImage= "thisisu2.png", Favorited = 1 },
                //    new MusicDTO { Id = 9 , AlbumName = "U2 POP", ArtistName = "U2" , MusicName = "Staring At The Sun"  , AlbumImage= "thisisu2.png"  , Favorited = 0 },
                //};

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task LoadPlayListExecute()
        {
            IsBusy = true;
            try
            {

                if (_bNotConnected)
                {
                    return;
                }


                AppSettings.PlayList = new PlayListDTO();

                var item = await PlayListsData.GetPlayListByDeviceIDAsync(AppSettings.Device.Id);

                if (item == null)
                {
                    PlayListDTO playList = new PlayListDTO();
                    playList.Device = AppSettings.Device;
                    playList.Musics = new List<MusicDTO>();
                    playList.PlayListName = "Favorite Songs";

                    var res = await PlayListsData.AddItemAsync(playList);

                    if (!res.Success)
                    {
                        return;
                    }

                    item = await PlayListsData.GetPlayListByDeviceIDAsync(AppSettings.Device.Id);

                }

                AppSettings.PlayList = item;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task GetDeviceInfo()
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

        public ObservableCollection<MusicDTO> GetSongs
        {
            get => _Musics;
            set
            {
                _Musics = value;
                SetProperty(ref _Musics, value);
            }
        }


        public string GetTextConnection
        {
            get => _sConnected;
            set {
                SetProperty(ref _sConnected, value);
            }
        }

        public bool IsNotConnected
        {
            get => _bNotConnected;
            set {
                SetProperty(ref _bNotConnected, value);
            }
        }

        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var current = Connectivity.NetworkAccess;
            var profiles = e.ConnectionProfiles;

            if (current == NetworkAccess.Internet)
            {
                IsNotConnected = false;
            }
            else
            {
                IsNotConnected = true;
            }

        }

    }
}
