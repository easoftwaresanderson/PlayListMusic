﻿using APIMusicPlayList.Infra.Data.EF.Data;
using APIMusicPlayLists.Core.Entities;
using APIMusicPlayLists.Core.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace APIMusicPlayList.Infra.Data.EF.Repositories
{
    public class PlayListRepository : EFRepository<PlayList>, IPlayListRepository
    {
        public PlayListRepository(MusicPlayListDBContext dbContext) : base(dbContext)
        {
        }
    }
}
