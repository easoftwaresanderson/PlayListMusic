using System;
using System.Collections.Generic;
using System.Text;

namespace APIMusicPlayLists.Infra.Shared.DTOs
{
    public class MusicDTO : BaseDTO
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
