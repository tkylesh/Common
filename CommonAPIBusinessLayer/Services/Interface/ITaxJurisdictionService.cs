using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonAPICommon.Dto;

namespace CommonAPIBusinessLayer.Services.Interface
{
    public interface ITaxJurisdictionService
    {
        JurisdictionResultDto GetTaxJurisdiction(string address, string city, string state, string zip, string effdate, string taxcodeselected);
    }
}
