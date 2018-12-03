using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCLCDKPaymentService.Models
{
    public class Response
    {
        public string Returkode { get; set; }
        public string Returtekst { get; set; }
        public string Fejlfelt { get; set; }
    }
}