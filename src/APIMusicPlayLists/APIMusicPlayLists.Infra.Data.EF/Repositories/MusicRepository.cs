using APIMusicPlayList.Infra.Data.EF.Data;
using APIMusicPlayLists.Core.Entities;
using APIMusicPlayLists.Core.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIMusicPlayList.Infra.Data.EF.Repositories
{
    public class MusicRepository : EFRepository<Music>, IMusicRepository
    {
        public MusicRepository(MusicPlayListDBContext dbContext) : base(dbContext)
        {

        }
    }
}
