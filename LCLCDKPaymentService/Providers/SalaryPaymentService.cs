using LCLCDKPaymentService.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LCLCDKPaymentService.Providers
{
    public class SalaryPaymentService
    {
        public bool ValidateResponseData(string stringResponse)
        {
            if (string.IsNullOrWhiteSpace(stringResponse)) return false;
            if (stringResponse == "{}") return false;
            return true;
        }

        public async Task<Response> MakeSalaryPayment(SalaryPayment payment)
        {
            HttpResponseMessage response = null;
            string responseJson = null;
            Response res = null;
            using (HttpClient client = new HttpClient())
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                    (se, cert, chain, sslerror) =>
                    {
                        return true;
                    };
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;

                try
                {
                    string requestBody = JsonConvert.SerializeObject(payment);

                    client.BaseAddress = new Uri("https://10.14.30.183:8600/findash/sb/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("X-DB-LSID", "");
                    client.DefaultRequestHeaders.Add("X-DB-CorrelId", "FINPAY"); //This will only work in Test-env - makes it possible to fetch data without valid LSID.. BUT ONLY IN TEST :)
                    client.DefaultRequestHeaders.Add("X-IBM-Client-Id", "2cad7bec-40c7-4a8b-a593-21d4b40d8f28");
                    client.DefaultRequestHeaders.Add("X-IBM-Client-Secret", "oW5fA4lH6sY5iA3eN3cS6yQ0aJ2wS6oH2gA5bA5oN0pI8kR6vS");

                    HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, "F9SalaryServiceV00/get");
                    req.Content = new StringContent(
                        requestBody,
                        Encoding.UTF8,
                        "application/json");

                    response = await client.SendAsync(req);
                    responseJson = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode && ValidateResponseData(responseJson))
                    {
                        //if (JsonConvert.DeserializeObject<dynamic>(responseJson).NumberOfTransactions > "0")
                        //{
                        //Json deserializer throws when {} orccurs. That is why we replace all {} with empty string. 
                        var tempResp = responseJson.Replace("{}", "\"\"");
                        res = JsonConvert.DeserializeObject<Response>(tempResp);
                        res.Returtekst = "Payment created - Archive reference: " + res.Returtekst;
                        //}
                        //else
                        //{
                        //    res = new Response
                        //    {
                        //        ReturnText = "No Transactions",
                        //        ReturnCode = "0",
                        //        FailureId = "0000000"
                        //    };
                        //}
                    }
                    else
                    {
                        res = new Response()
                        {
                            Returtekst = "ISLAY error",
                            Returkode = "500",
                            Fejlfelt = "0000000"
                        };
                    }

                    return res;
                }
                catch (Exception ex)
                {
                    throw ex;
                    #region ErrorLogging

                    #endregion
                    //return null;
                }
            }
        }
    }
}