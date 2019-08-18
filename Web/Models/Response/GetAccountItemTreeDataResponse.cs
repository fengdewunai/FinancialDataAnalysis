using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.Response
{
    public class GetAccountItemTreeDataResponse
    {
        public string id { get; set; }

        public string text { get; set; }

        public bool leaf { get; set; }

        public bool root { get; set; }

        public bool @checked { get; set; }
    }
}