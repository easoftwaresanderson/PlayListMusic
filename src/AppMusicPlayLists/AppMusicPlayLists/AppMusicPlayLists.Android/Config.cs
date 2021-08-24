using System;
using Xamarin.Forms;



[assembly: Dependency(typeof(AppMusicPlayLists.Droid.Config))]
namespace AppMusicPlayLists.Droid
{

    public class Config : IConfig
    {

        private string _diretorioDB;

        public Config()
        {
        }

        public string DirectoryDB
        {
            get
            {
                if (string.IsNullOrEmpty(_diretorioDB))
                {
                    _diretorioDB = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                }
                return _diretorioDB;
            }
        }
                
      
    }
}
