using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DataModel;

namespace IDataAccess
{
    /// <summary>
    /// 获取数据
    /// </summary>
    public interface IFinancialDataDAL
    {
        /// <summary>
        /// 获取所有科目
        /// </summary>
        /// <returns></returns>
        List<AccountItemModel> GetAllAccount();
    }
}
