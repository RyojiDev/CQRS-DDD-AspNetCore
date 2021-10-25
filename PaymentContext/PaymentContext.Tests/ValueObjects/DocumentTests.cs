using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Tests.ValueObjects
{
    [TestClass]
    public class DocumentTests
    {
        // Red, Green, Refactor

        [TestMethod]
        public void ShouldReturnErrorWhenCNPJIsInvalid()
        {
            var doc = new Document("1234", EDocumentType.CNPJ);
           Assert.IsTrue(!doc.IsValid);
        }

        [TestMethod]
          public void ShouldReturnSuccessrWhenCNPJIsValid()
        {
           var doc = new Document("58219012000191", EDocumentType.CNPJ);
           Assert.IsTrue(doc.IsValid);
        }

        [TestMethod]
          public void ShouldReturnErrorWhenCPFIsInvalid()
        {
           var doc = new Document("12312344", EDocumentType.CPF);
           Assert.IsTrue(!doc.IsValid);
        }

        [TestMethod]
        [DataTestMethod]
        [DataRow("53838460006")]
        [DataRow("48325205040")]
        [DataRow("24002596079")]
         public void ShouldReturnSuccessrWhenCPFIsValid(string cpf)
        {
           var doc = new Document(cpf, EDocumentType.CPF);
           Assert.IsTrue(doc.IsValid);
        }
    }
}