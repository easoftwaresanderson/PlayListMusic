using APIMusicPlayLists.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace APIMusicPlayLists.Core.Entities
{
    public class PlayList : BaseEntity
    {
        public string PlayListName { get; set; }

        [ForeignKey("DeviceID")]
        public int DeviceId { get; set; }
        [Required]
        public virtual Device Device { get; set; }

        public virtual ICollection<Music> Musics { get; set; }

    }
}
