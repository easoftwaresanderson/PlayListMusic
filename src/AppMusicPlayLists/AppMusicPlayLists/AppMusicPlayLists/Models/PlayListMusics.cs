using SQLite;
using System;

namespace AppMusicPlayLists.Models
{
    public class PlayListMusics
    {
        [PrimaryKey,Unique,AutoIncrement]
        public int LocalID { get; set; }
        public int PlayListId { get; set; }
        public int MusicId { get; set; }
        public string AlbumImage { get; set; }
        public string MusicName { get; set; }
        public string AlbumName { get; set; }


    }
}
