using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// form数据model
    /// </summary>
    public class FormDataModel
    {
        public bool success { get; set; }

        public List<Dictionary<string,string>> data { get; set; }
    }
}
