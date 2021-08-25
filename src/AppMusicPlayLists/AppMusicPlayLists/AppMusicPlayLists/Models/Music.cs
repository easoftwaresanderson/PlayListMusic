using APIMusicPlayLists.Infra.Shared.DTOs;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppMusicPlayLists.Models
{
    public class Music : MusicDTO
    {
        [PrimaryKey]
        public override int Id { get; set; }

    }
}
