using APIMusicPlayLists.Infra.Shared.Commands;
using AppMusicPlayLists.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace AppMusicPlayLists.Services.LocalServices
{
    public class LocalPlayListServices
    {
        private static SQLiteConnection _conexao;
        public LocalPlayListServices()
        {
            _conexao = ConnectionDB.GetConnection();
        }

        public bool GetQuery(ref PlayList Entity, string sQuery = "", object[] QueryParameters = null, string sOrder = "")
        {
            try
            {

                List<PlayList> ListRet = new List<PlayList>();


                string sSQL = "SELECT * FROM PlayList ";


                if (!String.IsNullOrEmpty(sQuery))
                {
                    sQuery = " WHERE " + sQuery;
                }

                if (!String.IsNullOrEmpty(sOrder))
                {
                    sOrder = " ORDER BY " + sOrder;
                }

                if (!String.IsNullOrEmpty(sQuery))
                {
                    sSQL += sQuery;
                }


                if (!String.IsNullOrEmpty(sOrder))
                {
                    sSQL += sOrder;
                }

                ListRet = _conexao.Query<PlayList>(sSQL, QueryParameters);

                if (ListRet.Count > 0)
                {
                    Entity = ListRet[0];
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fail to get data \nErr : " + ex.Message);
                return false;
            }

        }

        public PlayList ListByID(int id)
        {
            try
            {

                var list = _conexao.Query<PlayList>("SELECT * FROM PlayList WHERE id=" + id);

                if (list.Count > 0)
                {
                    return list[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine("Fail to get music data \nErr : " + ex.Message);
                return null;
            }

        }

        public ObservableCollection<PlayList> List()
        {
            try
            {
                ObservableCollection<PlayList> Lst = new ObservableCollection<PlayList>();

                var list = _conexao.Query<PlayList>("SELECT * FROM PlayList");

                Lst = new ObservableCollection<PlayList>(list);

                return Lst;
            }
            catch (Exception ex)
            {

                Debug.WriteLine("Fail to get music data \nErr : " + ex.Message);
                return null;
            }

        }
          
        public ObservableCollection<SyncMusics> GetSyncMusics()
        {
            try
            {
                ObservableCollection<SyncMusics> Lst = new ObservableCollection<SyncMusics>();

                var list = _conexao.Query<SyncMusics>("SELECT * FROM SyncMusics");

                Lst = new ObservableCollection<SyncMusics>(list);

                return Lst;
            }
            catch (Exception ex)
            {

                Debug.WriteLine("Fail to get syncmusic data \nErr : " + ex.Message);
                return null;
            }

        }

        public ObservableCollection<PlayListMusics> GetPlayListMusics(int PlayLisId)
        {
            try
            {
                ObservableCollection<PlayListMusics> Lst = new ObservableCollection<PlayListMusics>();

                var list = _conexao.Query<PlayListMusics>("SELECT * FROM PlayListMusics where PlayLisId = {0}", PlayLisId);

                Lst = new ObservableCollection<PlayListMusics>(list);

                return Lst;
            }
            catch (Exception ex)
            {

                Debug.WriteLine("Fail to get PlayListMusics data \nErr : " + ex.Message);
                return null;
            }

        }

        public bool AddSyncMusics(PlayListFavoriteCommand command)
        {

            SyncMusics reg = new SyncMusics
            {
                MusicId = command.MusicId,
                PlayListId = command.PlayListId,
                Favorite = command.Favorite
            };

            if (!ConnectionDB.Insert<SyncMusics>(ref reg))
            {
                Debug.WriteLine(String.Format("Fail to add sync music {0}", reg.MusicId));
                return false;
            }
            else
            {
                return true;
            }

        }

    }
}
