using APIMusicPlayLists.Infra.Shared.DTOs;
using AppMusicPlayLists.Models;
using AppMusicPlayLists.Services.LocalServices;
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

        private ObservableCollection<Music> _Musics;
        public Command LoadItemsCommand { get; }

        public FavoritesViewModel()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            
            _bNotConnected = !Utils.IsInternetAvaliable();
            IsNotConnected = _bNotConnected;

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            LoadItemsCommand.Execute(this);
        }



        public async Task ExecuteLoadItemsCommand()
        {

            IsBusy = true;

            try
            {
                if(_Musics==null)
                {
                    _Musics = new ObservableCollection<Music>();
                }
                else
                {
                    _Musics.Clear();
                }

                if(AppSettings.PlayList ==null)
                {
                    return;
                }          
                
                if(AppSettings.PlayList.Id ==0)
                {
                    return;
                } 

                LocalPlayListServices localPlayListServices = new LocalPlayListServices();
                
                var favorites = localPlayListServices.GetPlayListMusics(AppSettings.PlayList.Id);

                LocalMusicServices localMusicServices = new LocalMusicServices();

                foreach (PlayListMusics item in favorites)
                {
                    Music m = localMusicServices.ListByID(item.MusicId);

                    if(m!=null)
                         _Musics.Add(m);
                }

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


        public ObservableCollection<Music> GetSongs
        {
            get => _Musics;
            set
            {
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
