using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPIBusinessLayer.Services.Interface
{
    public interface ICoverageVerifierWorker
    {
        void Solve(string policyHolderName, string carrierName, DateTime effectiveDate);
    }
}
