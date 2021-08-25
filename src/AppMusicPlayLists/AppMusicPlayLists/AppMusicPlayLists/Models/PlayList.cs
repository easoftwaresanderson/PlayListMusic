using APIMusicPlayLists.Infra.Shared.DTOs;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppMusicPlayLists.Models
{
    public class PlayList 
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string PlayListName  { get; set; } 
        
    }
}
