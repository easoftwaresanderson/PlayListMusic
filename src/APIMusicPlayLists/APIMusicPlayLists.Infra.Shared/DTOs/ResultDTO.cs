using System;
using System.Collections.Generic;

namespace APIMusicPlayLists.Infra.Shared.DTOs
{
    public class ResultDTO
    {
        public string Action { get; set; }

        public bool Success
        {
            get { return Errors == null || Errors.Count == 0; }
        }
        public List<string> Errors { get; set; } = new List<string>();

        //public object Result { get; set; }
        //public string ResultJSON { get; set; }

        public string ReturnID { get; set; }
    }
}
