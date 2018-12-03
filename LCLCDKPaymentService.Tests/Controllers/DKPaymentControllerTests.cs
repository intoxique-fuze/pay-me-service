using Microsoft.VisualStudio.TestTools.UnitTesting;
using LCLCDKPaymentService.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LCLCDKPaymentService.Models;
using LCLCDKPaymentService.Providers;
using System.Web.Http;
using Moq;

namespace LCLCDKPaymentService.Controllers.Tests
{   
    [TestClass()]
    public class DKPaymentControllerTests
    {
        public Payment payment;
        public DKPaymentService client = new DKPaymentService();
        public Response r = null;
        public AccountTransferServiceController dkcontroller = new AccountTransferServiceController();

        [TestInitialize]
        public void ClassInitialize()
        {
            payment = new Payment();
            payment.AgreementID = "0F2714";
            payment.Amount = "0000000004";
            payment.FromAccount = "3361632362";
            payment.ToAccount = "3258186214";
            payment.UserId = "5G7283";
            payment.PaymentType = "BKT";            
        }

        [TestCleanup]
        public void ClassCleanUp()
        {            
            r = null;
        }

        [TestMethod()]
        public async void MakePaymentTest()
        {     
                 
            r = await client.MakePayment(payment);
            Assert.AreEqual(r.Returkode, "0");
            Assert.Fail();
        }

        [TestMethod()]
        public void DKPaymentControllerTest()
        {
            var mock = new Mock<AccountTransferServiceController>();

            //mock.Setup(x => x.MakePayment(It.IsAny<Payment>()));
            //dkcontroller.MakePayment(payment);

            //Assert.AreEqual(res.Status, TaskStatus.WaitingForActivation);
            mock.Verify(x => x.MakePayment(It.IsAny<Payment>()),Times.Exactly(1));
        }
    }
}