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
using AppMusicPlayLists.Services.LocalServices;

namespace AppMusicPlayLists.ViewModels
{
    public class SongsViewModel : BaseViewModel
    {
        private string _sConnected;
        private bool _bNotConnected;
        private IMusicServices MusicsData => DependencyService.Get<IMusicServices>();
        private IPlayListServices PlayListsData => DependencyService.Get<IPlayListServices>();

        private ObservableCollection<Music> _Musics;

        public Command LoadItemsCommand { get; }
        public Command ExecuteTaskCommand { get; }
        public Command LoadPlayListCommand { get; }
        public Command FavoriteCommand
        {
            get;
            set;
        }

        public Command SyncItemsCommand { get; }


        public SongsViewModel()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

            _bNotConnected = !Utils.IsInternetAvaliable();
            IsNotConnected = _bNotConnected;

            _Musics = new ObservableCollection<Music>();

            SyncItemsCommand = new Command(async () => await ExecuteSyncDataCommand());

            FavoriteCommand = new Command<Music>(OnFavoriteClicked);

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            //RefreshView already start execute
            //LoadItemsCommand.Execute(this);

            LoadPlayListCommand = new Command(async () => await ExecuteLoadPlayList());


            ExecuteTaskCommand = new Command(async () => await ExecuteTask());
            ExecuteTaskCommand.Execute(this);

        }

        private async Task ExecuteTask()
        {
            bool _bConnectedDB = ConnectionDB.OpenConnnection();

            var Task1 = Task.Run(
             async () =>
             {
                 return await ExecuteGetDeviceInfo();

             })
               .ContinueWith(
               Task2 =>
               {

                   if (_bConnectedDB)
                   {
                       SyncItemsCommand.Execute(this);
                   }


               }, TaskContinuationOptions.OnlyOnRanToCompletion)
               .ContinueWith(
               Task3 =>
               {
                   LoadPlayListCommand.Execute(this);

               }, TaskContinuationOptions.OnlyOnRanToCompletion);


        }

        async Task<bool> ExecuteGetDeviceInfo()
        {
            try
            {
                await AppSettings.GetDeviceInfo();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        async Task ExecuteSyncDataCommand()
        {
            try
            {
                SyncData syncData = new SyncData();
                await syncData.Execute();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async void OnFavoriteClicked(Music music)
        {
            try
            {
                if (music != null)
                {
                    int idx = _Musics.IndexOf(music);

                    if (idx >= 0)
                    {
                        if (AppSettings.PlayListMusics == null)
                        {
                            AppSettings.PlayListMusics = new ObservableCollection<PlayListMusics>();
                        }

                        PlayListFavoriteCommand command = new PlayListFavoriteCommand();
                        command.Favorite = music.Favorite == 0 ? 1 : 0;
                        command.MusicId = music.Id;
                        command.PlayListId = AppSettings.PlayList.Id;

                        if (_bNotConnected)
                        {
                            LocalPlayListServices localPlayListServices = new LocalPlayListServices();

                            if (!localPlayListServices.AddSyncMusics(command))
                            {
                                return;
                            }
                        }
                        else
                        {
                            var res = await PlayListsData.FavoriteSong(command);

                            if (!res.Success)
                            {
                                return;
                            }
                        }

                        music.Favorite = command.Favorite;

                        _Musics[idx] = music;

                        PlayListMusics playListMusic = new PlayListMusics
                        {
                            AlbumImage = music.AlbumImage,
                            AlbumName = music.AlbumName,
                            MusicName = music.MusicName,
                            MusicId = music.Id,
                            PlayListId = AppSettings.PlayList.Id,
                        };

                        var retMusic = AppSettings.PlayListMusics.Where(x => x.MusicId == music.Id).FirstOrDefault();

                        if (music.Favorite == 0)
                        {
                            if (retMusic != null)
                            {
                                AppSettings.PlayListMusics.Remove(retMusic);
                            }
                        }
                        else
                        {
                            if (retMusic == null)
                            {
                                AppSettings.PlayListMusics.Add(playListMusic);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine("Fail to Favorite/Unfavorite music.\nErr : " + ex.Message);
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
                    Music music = new Music
                    {
                        Id = item.Id,
                        MusicName = item.MusicName,
                        AlbumImage = item.AlbumImage,
                        AlbumName = item.AlbumName,
                        AlbumNotes = item.AlbumNotes,
                        AlbumYear = item.AlbumYear,
                        Favorite = item.Favorite,
                        ArtistName = item.ArtistName
                    };

                    _Musics.Add(music);
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

        async Task ExecuteLoadPlayList()
        {
            IsBusy = true;
            try
            {
                AppSettings.PlayList = new PlayList();


                if (_bNotConnected)
                {
                    return;
                }

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

                PlayList list = new PlayList
                {
                    Id = item.Id,
                    PlayListName = item.PlayListName
                };


                AppSettings.PlayList = list;

                if (item.Musics !=null)
                {
                    AppSettings.PlayListMusics = new ObservableCollection<PlayListMusics>();

                    foreach (MusicDTO music in item.Musics)
                    {
                        PlayListMusics playListMusic = new PlayListMusics
                        {
                            AlbumImage = music.AlbumImage,
                            AlbumName = music.AlbumName,
                            MusicName = music.MusicName,
                            MusicId = music.Id,
                            PlayListId = AppSettings.PlayList.Id,
                        };

                        AppSettings.PlayListMusics.Add(playListMusic);
                    }
                }

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



        public ObservableCollection<Music> GetSongs
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
            set
            {
                SetProperty(ref _sConnected, value);
            }
        }

        public bool IsNotConnected
        {
            get => _bNotConnected;
            set
            {
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
                ExecuteTaskCommand.Execute(this);
            }
            else
            {
                IsNotConnected = true;
            }

        }

    }
}
