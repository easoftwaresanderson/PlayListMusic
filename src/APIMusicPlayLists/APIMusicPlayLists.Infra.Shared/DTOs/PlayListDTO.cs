using System;
using System.Collections.Generic;
using System.Text;

namespace APIMusicPlayLists.Infra.Shared.DTOs
{
    public class PlayListDTO : BaseDTO
    {
        public string PlayListName { get; set; }
        public DeviceDTO Device { get; set; }
        public virtual ICollection<MusicDTO> Musics { get; set; }
    }
}
