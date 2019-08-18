using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataModel
{
    /// <summary>
    /// 动态grid返回的model
    /// </summary>
    public class DynamicGridDataModel
    {
        public int total { get; set; }

        public List<Dictionary<string, string>> rows { get; set; }
    }
}
