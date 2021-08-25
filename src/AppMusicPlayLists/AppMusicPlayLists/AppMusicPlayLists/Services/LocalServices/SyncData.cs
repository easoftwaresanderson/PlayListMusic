﻿using APIMusicPlayLists.Infra.Shared.Commands;
using APIMusicPlayLists.Infra.Shared.DTOs;
using AppMusicPlayLists.Models;
using AppMusicPlayLists.Services.ApiServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppMusicPlayLists.Services.LocalServices
{
    public class SyncData
    {

        private IMusicServices MusicsData => DependencyService.Get<IMusicServices>();
        private IPlayListServices PlayListData => DependencyService.Get<IPlayListServices>();

        public SyncData()
        {
        }

        public async Task SyncMusics()
        {
            //Desabilitar para acessar a API
            var items = await MusicsData.GetItemsAsync(true);

            if (items == null)
            {
                return;
            }

            LocalMusicServices localMusicServices = new LocalMusicServices();


            foreach (var item in items)
            {
                Music music = new Music();

                music = localMusicServices.ListByID(item.Id);

                if (music == null)
                {
                    music = new Music
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


                    if (!ConnectionDB.Insert<Music>(ref music))
                    {
                        Debug.WriteLine(String.Format("Fail to sync music {0}", music.Id));
                    }
                }
                else
                {
                    music = new Music
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

                    if (!ConnectionDB.Update<Music>(ref music))
                    {
                        Debug.WriteLine(String.Format("Fail to sync music {0}", music.Id));
                    }
                }
            }


        }


        public async Task SyncFavoritesMusic()
        {

            LocalPlayListServices servicePlayList = new LocalPlayListServices();

            var items = servicePlayList.GetSyncMusics();


            foreach (var item in items)
            {
                PlayListFavoriteCommand command = new PlayListFavoriteCommand();
                command.Favorite = item.Favorite;
                command.MusicId = item.MusicId;
                command.PlayListId = item.PlayListId;

                var res = await PlayListData.FavoriteSong(command);

                if (!res.Success)
                {
                    return;
                }

                if (!ConnectionDB.Delete<SyncMusics>(item))
                {
                    Debug.WriteLine(String.Format("Fail to sync music {0}", item.MusicId));
                }
            }

        }


        public async Task Execute()
        {
            try
            {
                bool _bConnected = Utils.IsInternetAvaliable();

                if (!_bConnected)
                {
                    Debug.WriteLine("No Internet avaliable to sync data.");
                    return;
                }

                ConnectionDB.BeginTransaction();

                var Task1 = Task.Run(
                 async () =>
                 {
                     await SyncFavoritesMusic();

                 }).ContinueWith(
                   async TaskSync2 =>
                   {
                       await SyncMusics();
                       
                   }, TaskContinuationOptions.OnlyOnRanToCompletion)
                   .ContinueWith(
                   async TaskSync3 =>
                   {
                       await SyncPlayLists();

                   }, TaskContinuationOptions.OnlyOnRanToCompletion); ;


                ConnectionDB.CommitTransaction();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                ConnectionDB.RollbackTransaction();
            }

        }

        public async Task SyncPlayLists()
        {
            //Desabilitar para acessar a API
            var data = await PlayListData.GetPlayListByDeviceIDAsync(AppSettings.Device.Id);

            if (data == null)
            {
                return;
            }

            LocalPlayListServices localPlayListServices = new LocalPlayListServices();


            PlayList playlist = new PlayList();

            playlist = localPlayListServices.ListByID(data.Id);

            if (playlist == null)
            {
                playlist = new PlayList
                {
                    Id = data.Id,
                    PlayListName = data.PlayListName
                };

                if (!ConnectionDB.Insert<PlayList>(ref playlist))
                {
                    Debug.WriteLine(String.Format("Fail to sync music {0}", playlist.Id));
                    return;
                }

                if (data.Musics != null)
                {
                    foreach (MusicDTO m in data.Musics)
                    {
                        PlayListMusics playListMusics = new PlayListMusics
                        {
                            PlayListId = playlist.Id,
                            AlbumImage = m.AlbumImage,
                            AlbumName = m.AlbumName,
                            MusicId = m.Id,
                            MusicName = m.MusicName
                        };

                        if (!ConnectionDB.Insert<PlayListMusics>(ref playListMusics))
                        {
                            Debug.WriteLine(String.Format("Fail to sync music {0}", playlist.Id));
                            return;
                        }
                    }

                }

            }
            else
            {
                playlist = new PlayList
                {
                    Id = data.Id,
                    PlayListName = data.PlayListName
                };

                if (!ConnectionDB.Update<PlayList>(ref playlist))
                {
                    Debug.WriteLine(String.Format("Fail to sync music {0}", playlist.Id));
                }

                ObservableCollection<PlayListMusics> playlistMusics = new ObservableCollection<PlayListMusics>();

                playlistMusics = localPlayListServices.GetPlayListMusics(data.Id);

                if (playlistMusics != null)
                {
                    foreach (PlayListMusics m in playlistMusics)
                    {

                        if (!ConnectionDB.Delete<PlayListMusics>(m))
                        {
                            Debug.WriteLine(String.Format("Fail to sync music {0}", playlist.Id));
                            return;
                        }
                    }
                }


                if (data.Musics != null)
                {
                    foreach (MusicDTO m in data.Musics)
                    {
                        PlayListMusics playListMusics = new PlayListMusics
                        {
                            PlayListId = playlist.Id,
                            AlbumImage = m.AlbumImage,
                            AlbumName = m.AlbumName,
                            MusicId = m.Id,
                            MusicName = m.MusicName
                        };

                        if (!ConnectionDB.Insert<PlayListMusics>(ref playListMusics))
                        {
                            Debug.WriteLine(String.Format("Fail to sync music {0}", playlist.Id));
                            return;
                        }

                    }
                }
            }

        }
    }
}