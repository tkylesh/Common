using CommonAPIDAL.DataAccess;
using CommonAPIDAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPIDAL.Repository.Impl
{
    public class CreditOrderRepository : ICreditOrderRepository
    {
        public int SearchCreditOrderBySSN_DL(dynamic searchInfo)
        {
            return CreditDataAccess.SearchCreditOrderBySSN_DL(searchInfo);
        }


        public int SearchCreditOrderBySSN(dynamic searchInfo)
        {
            return CreditDataAccess.SearchCreditOrderBySSN(searchInfo);
        }


        public int SearchCreditOrderByDL(dynamic searchInfo)
        {
            return CreditDataAccess.SearchCreditOrderByDL(searchInfo);
        }


        public int SearchByCreditOrderByName_Address(dynamic searchInfo)
        {
            return CreditDataAccess.SearchByCreditOrderByName_Address(searchInfo);
        }

        public string GetCreditPolType(int rmid)
        {
            return CreditDataAccess.GetCreditPolType(rmid);
        }
    }
}
