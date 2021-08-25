using APIMusicPlayLists.Infra.Shared.Commands;
using APIMusicPlayLists.Infra.Shared.DTOs;
using AppMusicPlayLists.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace AppMusicPlayLists.Services.LocalServices
{
    public class LocalMusicServices
    {
        private static SQLiteConnection _conexao;
        public LocalMusicServices()
        {
            _conexao = ConnectionDB.GetConnection();
        }

        public bool GetQuery(ref Music Entity, string sQuery = "", object[] QueryParameters = null, string sOrder = "")
        {
            try
            {

                List<Music> ListRet = new List<Music>();


                string sSQL = "SELECT * FROM Music ";


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

                ListRet = _conexao.Query<Music>(sSQL, QueryParameters);

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

        public Music ListByID(int id)
        {
            try
            {

                var list = _conexao.Query<Music>("SELECT * FROM Music WHERE id=" + id);

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

        public ObservableCollection<Music> List()
        {
            try
            {
                ObservableCollection<Music> Lst = new ObservableCollection<Music>();

                var list = _conexao.Query<Music>("SELECT * FROM Music");

                Lst = new ObservableCollection<Music>(list);

                return Lst;
            }
            catch (Exception ex)
            {

                Debug.WriteLine("Fail to get music data \nErr : " + ex.Message);
                return null;
            }

        }


       
    }
}
