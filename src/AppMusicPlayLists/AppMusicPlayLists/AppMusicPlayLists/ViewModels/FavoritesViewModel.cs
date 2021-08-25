using APIMusicPlayLists.Infra.Shared.DTOs;
using AppMusicPlayLists.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppMusicPlayLists.ViewModels
{
    public class FavoritesViewModel  : BaseViewModel
    {
        private bool _bNotConnected;

        private ObservableCollection<PlayListMusics> _Musics;
        public Command LoadItemsCommand { get; }

        public FavoritesViewModel()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

            _Musics = new ObservableCollection<PlayListMusics>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            //LoadItemsCommand.Execute(this);
        }



        public async Task ExecuteLoadItemsCommand()
        {

            IsBusy = true;

            try
            {
                _Musics.Clear();

                // TODO: Implementando o serviço de PlayList
                // var items = await MusicsData.GetItemsAsync(true);

                if (AppSettings.PlayListMusics !=null)
                {
                    foreach (PlayListMusics item in AppSettings.PlayListMusics)
                    {
                        _Musics.Add(item);
                    }
                }

                GetSongs = _Musics;

                //foreach (var item in items)
                //{
                //    _Musics.Add(item);
                //}

                //_Musics = new ObservableCollection<MusicDTO>()
                //{
                //    new MusicDTO { Id = 5 , AlbumName = "Ten", ArtistName = "Pearl Jam" , MusicName = "Come Back", AlbumImage= "pearljam.jpg"  },
                //    new MusicDTO { Id = 6 , AlbumName = "This is U2", ArtistName = "U2" , MusicName = "One", AlbumImage= "thisisu2.png"  },
                //    new MusicDTO { Id = 8 , AlbumName = "U2 POP", ArtistName = "U2" , MusicName = "If God Will Send His Angels" , AlbumImage= "thisisu2.png", Favorited = 1 },
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


        public ObservableCollection<PlayListMusics> GetSongs
        {
            get => _Musics;
            set
            {
                _Musics = value;
                SetProperty(ref _Musics, value);
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
            }
            else
            {
                IsNotConnected = true;
            }

        }

    }
}
