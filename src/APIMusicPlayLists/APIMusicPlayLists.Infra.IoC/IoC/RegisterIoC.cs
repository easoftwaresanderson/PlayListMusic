using APIMusicPlayList.Infra.Data.EF.Data;
using APIMusicPlayList.Infra.Data.EF.Repositories;
using APIMusicPlayLists.Core.Interfaces.IRepositories;
using APIMusicPlayLists.Core.Interfaces.IServices;
using APIMusicPlayLists.Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIMusicPlayLists.Infra.IoC
{
    public static class RegisterIoC
    {
        public static void RegisterServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped(typeof(IRepositoryBase<>), typeof(EFRepository<>));

            
            services.AddScoped<IDeviceRepository, DeviceRepository>();
            services.AddScoped<IDeviceServices, DeviceServices>();
            
            services.AddScoped<IMusicRepository, MusicRepository>();
            services.AddScoped<IMusicServices, MusicServices>();


            services.AddScoped<IPlayListRepository, PlayListRepository>();
            services.AddScoped<IPlayListServices, PlayListServices>();            
            

            //var strConnection = configuration.GetConnectionString("SqliteConnectionString");

            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            var strConnection = $"Data Source={path}{System.IO.Path.DirectorySeparatorChar}MusicPlayLists.db";

            services.AddDbContext<MusicPlayListDBContext>(options =>
                options.UseSqlite(strConnection)
            );

        }



    }
}
