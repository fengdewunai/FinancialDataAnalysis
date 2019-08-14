using IDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess
{
    public class TestDAL : ITestDAL
    {
        public int getA()
        {
            return 1;
        }
    }
}
