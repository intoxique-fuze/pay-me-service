using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LCLCDKPaymentService.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LCLCDKPaymentService.Providers
{
    public class DKPaymentService
    {
        public bool ValidateResponseData(string stringResponse)
        {
            if (string.IsNullOrWhiteSpace(stringResponse)) return false;
            if (stringResponse == "{}") return false;
            return true;
        }

        public async Task<Response> MakePayment(Payment payment)
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
                    string requestBody = JsonConvert.SerializeObject(new
                    {
                        OBJEKT = "Betaling  ",
                        AKTION = "BEBKTVal  ",
                        BRGNR = payment.UserId, //"5G7283", //
                        DATOFORMAT = "6",
                        DECIMALSEP = ",",
                        TIMEZONEID = "3923",
                        LANDEKODE = "DK",
                        PRODUKT = "BN ",
                        BRANDID = "DB ",
                        IP_DATA_GR = "DABA",
                        AFTALE = payment.AgreementID,//"0F2714", //
                        INTEKSTBRUGER = "E",
                        LDKD_BRUGER = "DK",
                        SPKD_BRUGER = "DA",
                        TZID_BRUGER = "3923",
                        BetalingsKategori = payment.PaymentType, //"BKT", //
                        DialogStatus = "Val",
                        BetalingsFunktion = "Opret",
                        BetKategori = payment.PaymentType,//"BKT", // 
                        FraKonto = payment.FromAccount, //"3361632362", //
                        TilInternKonto = "0000000000",
                        TilKonto = payment.ToAccount + "                                ", //"3258186214                                ", //
                        Belob = payment.Amount.PadLeft(10,'0'), //"0000000004", //
                        AntalDec = "0",
                        DSValuta = "DKK",
                        Valutakode = "DKK",
                        TransferTypeID = "BN 9048",
                        BeneficiaryType = payment.BeneficiaryType + "                                                                                                 "
                    });

                    client.BaseAddress = new Uri("https://10.14.30.183:8600/mobilebusiness/sb/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("X-DB-LSID", "");
                    client.DefaultRequestHeaders.Add("X-DB-CorrelId", "MOBANK3"); //This will only work in Test-env - makes it possible to fetch data without valid LSID.. BUT ONLY IN TEST :)
                    client.DefaultRequestHeaders.Add("X-IBM-Client-Id", "5487284e-9bca-4ca7-9d7b-b37f542fc62c");
                    client.DefaultRequestHeaders.Add("X-IBM-Client-Secret", "kL4iV5qL4hQ6iV8xX1pM0mU0sP3mG4bD0yV7sN7nT5hV8iX4yO");

                    HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, "LCLCDKPaymentServiceV00/get");
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
                    #region ErrorLogging

                    #endregion
                    throw ex;
                }
            }
        }
    }
}