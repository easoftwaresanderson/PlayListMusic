using APIMusicPlayLists.Infra.Shared.DTOs;
using AppMusicPlayLists.Models;
using AppMusicPlayLists.Services.ApiServices;
using AppMusicPlayLists.Services.LocalServices;
using Plugin.DeviceInfo;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppMusicPlayLists
{
    public partial class App : Application
    {
        public Command SyncItemsCommand { get; }

        private IMusicServices MusicsData => DependencyService.Get<IMusicServices>();

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MusicServices>();
            DependencyService.Register<PlayListServices>();
            DependencyService.Register<DeviceServices>();

            SyncItemsCommand = new Command(async () => await ExecuteSyncDataCommand());

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            bool _bConnected = ConnectionDB.OpenConnnection();

            if (_bConnected)
            {
                SyncItemsCommand.Execute(this);
            }


        }

        async Task ExecuteSyncDataCommand()
        {
            try
            {

                bool _bConnected = Utils.IsInternetAvaliable();

                if (!_bConnected)
                {
                    Debug.WriteLine("No Internet avaliable to sync data.");
                    return;
                }

                //Desabilitar para acessar a API
                var items = await MusicsData.GetItemsAsync(true);

                if (items == null)
                {
                    return;
                }

                LocalMusicServices localMusicServices = new LocalMusicServices();


                ConnectionDB.BeginTransaction();

                foreach (var item in items)
                {
                    MusicDTO music = new MusicDTO();

                    music = localMusicServices.ListByID(item.Id);

                    if (music == null)
                    {
                        music = new MusicDTO
                        {
                            Id = item.Id,
                            AlbumImage = item.AlbumImage,
                            AlbumName = item.AlbumName,
                            AlbumNotes = item.AlbumNotes,
                            AlbumYear = item.AlbumYear,
                            ArtistName = item.ArtistName,
                            Favorite = item.Favorite,
                            MusicName = item.MusicName
                        };


                        if (!ConnectionDB.Insert<MusicDTO>(ref music))
                        {
                            Debug.WriteLine(String.Format("Fail to sync music {0}", music.Id));
                        }
                    }
                    else
                    {
                        music = new MusicDTO
                        {
                            Id = item.Id,
                            AlbumImage = item.AlbumImage,
                            AlbumName = item.AlbumName,
                            AlbumNotes = item.AlbumNotes,
                            AlbumYear = item.AlbumYear,
                            ArtistName = item.ArtistName,
                            Favorite = item.Favorite,
                            MusicName = item.MusicName
                        };

                        if (!ConnectionDB.Update<MusicDTO>(ref music))
                        {
                            Debug.WriteLine(String.Format("Fail to sync music {0}", music.Id));
                        }
                    }


                }

                ConnectionDB.CommitTransaction();

            }
            catch (Exception ex)
            {
                ConnectionDB.RollbackTransaction();
                Debug.WriteLine(ex);
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
