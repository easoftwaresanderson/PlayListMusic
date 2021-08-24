using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppMusicPlayLists.iOS.Config))]

namespace AppMusicPlayLists.iOS
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
                    var diretorio = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                    _diretorioDB = Path.Combine(diretorio, "..", "Library");
                }
                return _diretorioDB;
            }
        }

 
    }
}