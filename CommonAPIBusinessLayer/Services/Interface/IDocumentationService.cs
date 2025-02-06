using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPIBusinessLayer.Services.Interface
{
    public interface IDocumentationService
    {
        void GetAlfaMembershipIdCard(byte[] pdf, int quoteId, string policyNumber);
    }
}
