using LCLCDKPaymentService.Models;
using LCLCDKPaymentService.Providers;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace LCLCDKPaymentService.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AccountTransferController : ApiController
    {

        Response r = null;
        [HttpPost]
        public async Task<IHttpActionResult> SalaryPayment([FromBody] SalaryPayment payment)
        {
            try
            {
                SalaryPaymentService client = new SalaryPaymentService();
                r = await client.MakeSalaryPayment(payment);
            }
            catch (Exception)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
            return Ok(r);
        }
    }
}
