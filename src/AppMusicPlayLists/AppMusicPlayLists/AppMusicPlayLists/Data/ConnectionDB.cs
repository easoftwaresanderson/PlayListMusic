using APIMusicPlayLists.Infra.Shared.DTOs;
using SQLite;
using System;
using System.Diagnostics;
using System.IO;
using Xamarin.Forms;

namespace AppMusicPlayLists
{
    public static class ConnectionDB
    {
        private static SQLiteConnection _conexao;

        private static string _sCaminhoDB = string.Empty;
        private static bool _bConnectionOpen;
        private static bool _bTrnAberta;

        public static bool AbreConexaoDB(bool bdDemonstracao = false)
        {
            try
            {
                var config = DependencyService.Get<IConfig>();
                
              
                _sCaminhoDB = Path.Combine(config.DirectoryDB, "playlistmusics.db3");
                        

                _conexao = new SQLiteConnection(_sCaminhoDB);

                if (!VerificaDB())
                {
                    return false;
                }

                _bConnectionOpen = true;

                return true;

            }
            catch (DllNotFoundException ex)
            {
                Debug.Write("Biblioteca SQLite não localizada.\nErro:" + ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }


        private static bool VerificaDB()
        {
            try
            {
                _conexao.CreateTable<MusicDTO>();

                _conexao.CreateTable<PlayListDTO>();

                return true;
            }
            catch (Exception ex)
            {
                Debug.Write("Falha ao criar banco de dados.\n" + ex.Message);
                return false;
            }

        }

        public static bool NextID<T>(string sNmField, ref long lngID, string sCriterio = "", object[] ParametrosCriterio = null) where T : class
        {
            try
            {
                string sSQL = "select ifnull(max(" + sNmField + "),0) from " + typeof(T).Name;

                if (!String.IsNullOrEmpty(sCriterio))
                {
                    sSQL = sSQL + " WHERE " + sCriterio;
                }

                if (ParametrosCriterio == null)
                {
                    lngID = _conexao.ExecuteScalar<long>(sSQL);
                }
                else
                {
                    lngID = _conexao.ExecuteScalar<long>(sSQL, ParametrosCriterio);
                }

                lngID += 1;

                return true;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static SQLiteConnection GetConnection()
        {
            return _conexao;
        }


        public static bool IsConnectionOpen()
        {
            if (_bConnectionOpen && _conexao != null)
            {
                return true;
            }

            return false;
        }

        public static bool BeginTransaction()
        {
            try
            {
                _conexao.BeginTransaction();
                _bTrnAberta = true;
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static bool CommitTransaction()
        {
            try
            {
                _conexao.Commit();
                _bTrnAberta = false;
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static bool RollbackTransaction()
        {
            try
            {
                if (_bTrnAberta)
                {
                    _conexao.Rollback();
                }

                _bTrnAberta = false;

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public static void CloseConnection()
        {
            _conexao.Dispose();
            _bConnectionOpen = false;
            _bTrnAberta = false;
        }

        public static bool Insert<T>(ref T Entity) where T : class
        {
            try
            {
                _conexao.Insert(Entity);
                return true;
            }
            catch (Exception)
            {

                throw;
            }

        }


        public static bool Update<T>(ref T Entity)
        {
            try
            {
                _conexao.Update(Entity);
                return true;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public static bool Delete<T>(ref T Entity)
        {

            try
            {
                _conexao.Delete(Entity);
                return true;
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
