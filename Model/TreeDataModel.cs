using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// Ext树的返回数据
    /// </summary>
    public class TreeDataModel
    {
        public string id { get; set; }

        public string text { get; set; }

        public bool leaf { get; set; }

        public bool root { get; set; }

        public bool @checked { get; set; }
    }
}
