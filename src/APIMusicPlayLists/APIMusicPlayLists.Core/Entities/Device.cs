using APIMusicPlayLists.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace APIMusicPlayLists.Core.Entities
{
    public class Device : BaseEntity
    {
        public string UniqueID { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string VersionString { get; set; }
        public string Platform { get; set; }
        public string Idiom { get; set; }
        public string DeviceType { get; set; }

        [ForeignKey("PlayListID")]
        public int? PlayListID { get; set; }
        public virtual PlayList PlayList { get; set; }

    }
}
