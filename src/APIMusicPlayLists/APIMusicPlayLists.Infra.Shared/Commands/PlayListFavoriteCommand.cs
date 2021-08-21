using System;
using System.Collections.Generic;
using System.Text;

namespace APIMusicPlayLists.Infra.Shared.Commands
{
    public class PlayListFavoriteCommand
    {
        public int PlayListId { get; set; }
        public int MusicId { get; set; }
        public int Favorite { get; set; }

    }
}
