using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonAPIBusinessLayer.Services.Interface;
using CommonAPICommon.Dto;
using CommonAPIDAL.Repository.Impl;

namespace CommonAPIBusinessLayer.Services.Impl
{
    public class CreditOrderService : ICreditOrderService
    {
        CreditOrderRepository repository = new CreditOrderRepository();

        public int OrderCredit(CreditOrderDto creditOrderDto)
        {

            //check for existing orders
            int rmId = PreviousOrderCheck(creditOrderDto);

            //order if need to
            if (rmId == 0)
            {
                rmId = CallWebService(creditOrderDto);
            }

            return rmId;
        }

        private int CallWebService(CreditOrderDto creditOrderInfo)
        {
            int rmId = 0;
            var ws = new CPInquiry.CPInquiryWebserviceClient();

            string dob = creditOrderInfo.DOB.ToString("MM/dd/yyyy");
            rmId = ws.submitCPRequestQAEnv(creditOrderInfo.ClientId.ToString(),
                                           creditOrderInfo.NamePrefix = creditOrderInfo.NamePrefix == null ? string.Empty : creditOrderInfo.NamePrefix,
                                           creditOrderInfo.NameFirst,
                                           creditOrderInfo.NameMiddle,
                                           creditOrderInfo.NameLast,
                                           creditOrderInfo.NameSuffix = creditOrderInfo.NameSuffix == null ? string.Empty : creditOrderInfo.NameSuffix,
                                           dob,
                                           creditOrderInfo.Age = creditOrderInfo.Age == null ? string.Empty : creditOrderInfo.Age,
                                           creditOrderInfo.Sex,
                                           creditOrderInfo.SSN,
                                           creditOrderInfo.HouseNumber = creditOrderInfo.HouseNumber == null ? string.Empty : creditOrderInfo.HouseNumber,
                                           creditOrderInfo.StreetName,
                                           creditOrderInfo.ApartmentNumber = creditOrderInfo.ApartmentNumber == null ? string.Empty : creditOrderInfo.ApartmentNumber,
                                           creditOrderInfo.City,
                                           creditOrderInfo.State,
                                           creditOrderInfo.Zip,
                                           creditOrderInfo.ZipPlus4 = creditOrderInfo.ZipPlus4 == null ? string.Empty : creditOrderInfo.ZipPlus4,
                                           creditOrderInfo.LicenseNumber,
                                           creditOrderInfo.LicenseState,
                                           creditOrderInfo.ProductArray,
                                           creditOrderInfo.AccountNumber,
                                           "",
                                           creditOrderInfo.DevEnv);
            return rmId;
        }

        private int PreviousOrderCheck(CreditOrderDto creditOrderInfo)
        {
            int rmId = 0;

            if (!string.IsNullOrWhiteSpace(creditOrderInfo.SSN) && !string.IsNullOrWhiteSpace(creditOrderInfo.LicenseNumber))
            {
                rmId = repository.SearchCreditOrderBySSN_DL(creditOrderInfo);
            }

            if (rmId == 0)
            {
                if (!string.IsNullOrWhiteSpace(creditOrderInfo.SSN))
                {
                    //search by SSN
                    rmId = repository.SearchCreditOrderBySSN(creditOrderInfo);
                }
                else if (!string.IsNullOrWhiteSpace(creditOrderInfo.LicenseNumber))
                {
                    //search by License Number
                    rmId = repository.SearchCreditOrderByDL(creditOrderInfo);
                }
            }

            //search by name, address if rmID is still 0
            if (rmId == 0)
            {
                rmId = repository.SearchByCreditOrderByName_Address(creditOrderInfo);
            }

            if (rmId != 0)
            {
                if (repository.GetCreditPolType(rmId) == string.Empty)
                {
                    rmId = 0;
                }
            }

            return rmId;
        }
    }
}
