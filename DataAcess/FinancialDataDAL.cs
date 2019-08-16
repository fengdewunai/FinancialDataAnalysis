using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using IDataAccess;
using Model.DataModel;

namespace DataAcess
{
    /// <summary>
    /// 获取数据
    /// </summary>
    public class FinancialDataDAL : DataAccessBase, IFinancialDataDAL
    {
        /// <summary>
        /// 获取所有科目
        /// </summary>
        /// <returns></returns>
        public List<AccountModel> GetAllAccount()
        {
            return CurrentConnectStringContext.StoredProcedure("Account_Get").QueryMany<AccountModel>();
        }
    }
}
