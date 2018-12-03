using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCLCDKPaymentService.Models
{
    public class Payment
    {
        public string UserId { get; set; }
        public string AgreementID { get; set; }
        public string FromAccount { get; set; }
        public string PaymentType { get; set; }
        public string ToAccount { get; set; }
        public string Amount { get; set; }
        public string BeneficiaryType { get; set; }
    }
}