using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppMusicPlayLists.Models
{
    public class SyncMusics 
    {
      
        [PrimaryKey, Unique, AutoIncrement]
        public int LocalID { get; set; }
        public int PlayListId { get; set; }      
        public int MusicId { get; set; }
        public int Favorite { get; set; }
    }

}
