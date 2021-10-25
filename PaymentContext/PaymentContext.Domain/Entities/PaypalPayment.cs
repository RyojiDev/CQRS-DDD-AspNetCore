using System;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Domain.Entities
{
    public class PaypalPayment : Payment
    {
        public PaypalPayment(
            string transactionCode,
            DateTime paidDate,
            DateTime expireDate,
            decimal total,
            decimal totalPaid,
            string payer,
            Address address,
            Email email,
            Document document) : base(
            paidDate,
            expireDate,
            total,
            totalPaid,
            payer,
            address, 
            email,
            document)
        {
            TransactionCode = transactionCode;
        }

        public string TransactionCode { get; private set; }

    }
}