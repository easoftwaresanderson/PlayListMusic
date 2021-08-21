using APIMusicPlayList.Infra.Data.EF.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIMusicPlayLists.Infra.Data.EF.Data
{
    public class MusicPlayListDbContextFactory : IDesignTimeDbContextFactory<MusicPlayListDBContext>
    {
        public MusicPlayListDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MusicPlayListDBContext>();
            //optionsBuilder.UseSqlite("Data Source=MusicPlayLists.db");

            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            var stringConection = $"Data Source={path}{System.IO.Path.DirectorySeparatorChar}MusicPlayLists.db";

            optionsBuilder.UseSqlite(stringConection);

            return new MusicPlayListDBContext(optionsBuilder.Options);
        }
    }
}
