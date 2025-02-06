using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonAPICommon.Dto;

namespace CommonAPIBusinessLayer.Services.Interface
{
    public interface IRAPA2Service
    {
        Task<VINResultDto> GetVINResults(string vin, Dictionary<string, string> RapaParmDict, string Rapaparm);
        void SetSelectedRapa2Vin(int quoteid, string vin, int seq);
    }
}
