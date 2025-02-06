using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPIBusinessLayer.Services.Impl
{
    public class ALIAPendingPaymentDto
    {
        public string MemberNumber { get; set; }
        public string PendingPaymentStatus { get; set; }
        public DateTime? PendingPaymentExpirationDate { get; set; }
        public string TransactionId { get; set; }
    }

    public class PendingPayment
    {
        public string Status { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
