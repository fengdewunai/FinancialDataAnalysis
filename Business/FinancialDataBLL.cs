using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBusiness;
using IDataAccess;
using Model.DataModel;

namespace Business
{
    /// <summary>
    /// 获取数据
    /// </summary>
    public class FinancialDataBLL : IFinancialDataBLL
    {
        /// <summary>
        /// 获取数据数据层
        /// </summary>
        private IFinancialDataDAL _financialDataDal;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="financialDataDal"></param>
        public FinancialDataBLL(IFinancialDataDAL financialDataDal)
        {
            _financialDataDal = financialDataDal;
        }

        /// <summary>
        /// 获取所有科目
        /// </summary>
        /// <returns></returns>
        public List<AccountItemModel> GetAllAccount()
        {
            return _financialDataDal.GetAllAccount();
        }
    }
}
