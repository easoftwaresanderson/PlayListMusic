using APIMusicPlayLists.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace APIMusicPlayLists.Core.Entities
{
    public class Music :BaseEntity
    {
        public string MusicName { get; set; }
        public string ArtistName { get; set; }
        public string AlbumImage { get; set; }
        public string AlbumName { get; set; }
        public int AlbumYear { get; set; }
        public string AlbumNotes { get; set; }
        public int Favorite { get; set; }


    }
}
