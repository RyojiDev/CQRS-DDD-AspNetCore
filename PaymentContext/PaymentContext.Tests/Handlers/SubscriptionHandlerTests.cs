using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Handlers;
using PaymentContext.Tests.Mocks;

namespace PaymentContext.Tests.Handlers
{
    [TestClass]
    public class SubscriptionHandlerTests
    {
        [TestMethod]
        public void ShouldReturnErrorWhenDocumentExists()
        {
            var handler = new SubscriptionHandler(new FakeStudentRepository(), new FakeEmailService());
            var command = new CreateBoletoSubscriptionCommand();

            command.FirstName = "Bruce";
            command.LastName = "Wayne";
            command.BarCode = "9988998898789789";
            command.Document = "99999999";
            command.Email = "ryoji@mail.com";
            command.BoletoNumber = "12345678";
            command.PaymentNumber = "123121";
            command.PaidDate = DateTime.Now;
            command.ExpireDate = DateTime.Now.AddMonths(1);
            command.Total = 60;
            command.TotalPaid = 60; 
            command.Payer = "Wayne Corp";
            command.PayerDocument = "12345678911";
            command.PayerDocumentType =  EDocumentType.CPF;
            command.PayerEmail = "batman@dc.com";
            command.Street = "Rua 10";
            command.Number = "asqwqw1";
            command.Neighborhood = "Gothan neighbohood";
            command.City = "Gothan city";
            command.State = "Kansas";
            command.Country = "Brasil";
            command.ZipCode = "60524550";

            handler.Handle(command);

            Assert.AreEqual(false, handler.Valid);
        }
    }
}