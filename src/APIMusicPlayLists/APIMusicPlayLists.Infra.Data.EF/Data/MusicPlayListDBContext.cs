using APIMusicPlayLists.Core.Entities;
using APIMusicPlayLists.Infra.Data.EF.Config;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIMusicPlayList.Infra.Data.EF.Data
{
    public class MusicPlayListDBContext : DbContext
    {
        public string DbPath { get; private set; }

        public DbSet<Device> Device { get; set; }
        public DbSet<Music> Music { get; set; }
        public DbSet<PlayList> PlayList { get; set; }


        public MusicPlayListDBContext(DbContextOptions<MusicPlayListDBContext> options) : base(options)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = $"Data Source={path}{System.IO.Path.DirectorySeparatorChar}MusicPlayLists.db";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = $"Data Source={path}{System.IO.Path.DirectorySeparatorChar}MusicPlayLists.db";
            optionsBuilder.UseSqlite(DbPath);
        }


        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {

            modelbuilder.ApplyConfiguration(new DeviceConfig());
            modelbuilder.ApplyConfiguration(new PlayListConfig());

            base.OnModelCreating(modelbuilder);

        }
    }
}
