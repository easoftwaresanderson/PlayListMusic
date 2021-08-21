using System;
using System.Collections.Generic;
using System.Text;

namespace APIMusicPlayLists.Infra.Shared.DTOs
{
    public class DeviceDTO : BaseDTO
    {
        public string UniqueID { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string VersionString { get; set; }
        public string Platform { get; set; }
        public string Idiom { get; set; }
        public string DeviceType { get; set; }

    }
}
