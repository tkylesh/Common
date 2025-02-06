using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPIDAL.Repository.Interface
{
    public interface ICreditOrderRepository
    {
        int SearchCreditOrderBySSN_DL(dynamic searchInfo);
        int SearchCreditOrderBySSN(dynamic searchInfo);
        int SearchCreditOrderByDL(dynamic searchInfo);
        int SearchByCreditOrderByName_Address(dynamic searchInfo);
        string GetCreditPolType(int rmid);
    }
}
