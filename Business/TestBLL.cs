using IBusiness;
using IDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class TestBLL: ITestBLL
    {
        private readonly ITestDAL _dal;

        public TestBLL(ITestDAL dal)
        {
            _dal = dal;
        }

        public int getA()
        {
            return _dal.getA();
        }
    }
}
