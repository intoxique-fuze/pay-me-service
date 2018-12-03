using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LCLCDKPaymentService.Models;
using LCLCDKPaymentService.Providers;
using System.Threading.Tasks;
using System.Web.Http.Cors;

namespace LCLCDKPaymentService.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AccountTransferServiceController : ApiController
    {
        Response r = null;
        [HttpPost]
        public async Task<IHttpActionResult> MakePayment([FromBody] Payment payment)
        {
            try
            {
                DKPaymentService client = new DKPaymentService();
                r = await client.MakePayment(payment);                
            }
            catch(Exception)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
            return Ok(r);
        }
    }
}
