using System;
using Flunt.Notifications;
using Flunt.Validations;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Repositories;
using PaymentContext.Domain.Services;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Commands;
using PaymentContext.Shared.Handlers;

namespace PaymentContext.Domain.Handlers
{
    public class SubscriptionHandler :
        Notifiable,
        IHandler<CreateBoletoSubscriptionCommand>,
        IHandler<CreatePaypalSubscriptionCommand>
    {
        private readonly IStudentRepository _repository;
        private readonly IEmailService _emailService;

        public SubscriptionHandler(IStudentRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public ICommandResult Handle(CreateBoletoSubscriptionCommand command)
        {
            // Fail Fest Validations
            command.Validate();
            if(command.Invalid)
            {
                AddNotifications(command);
                return new CommandResult(false,"Não foi possível realizar sua assinatura");
            }

            // Verificar se Document já está cadastrado
            if(_repository.DocumentExists(command.Document))
                AddNotification("Document", "Este CPF já está em uso");
            // Verificar se E-mail já está cadastrado
            if(_repository.DocumentExists(command.Email))
                AddNotification("Email", "Este Email já está em uso");
            // Gerar os VOS
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode); 
          
            // Gerar as Entidades

            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new BoletoPayment(command.BarCode,
                command.BoletoNumber,
                command.PaidDate,
                command.ExpireDate,
                command.Total,
                command.TotalPaid,
                command.Payer,
                address,
                email,
                new Document(command.PayerDocument,command.PayerDocumentType));

            // Relacionamentos
            subscription.AddPayment(payment);    
            student.AddSubscription(subscription);


            // Agrupar as validações

            AddNotifications(name, document, email, address, student, subscription, payment);

            // Checar as notificações
            if(Invalid)
                return new CommandResult(false, "Não foi possivel realizar sua assinatura");

            // Salvar as informações
            _repository.CreateSubscription(student);

            // Enviar E-mail de boas vindas
            _emailService.Send(student.Name.ToString(), student.Email.Address, "Incrição realizada com sucesso", "Seja bem vindo a nossa plataforma. Sua assinatura foi criada com sucesso");

            // Retornar informações
            return new CommandResult(true, "Assinatura cadastrada com sucesso");
        }

        public ICommandResult Handle(CreatePaypalSubscriptionCommand command)
        {

            // Verificar se Document já está cadastrado
            if(_repository.DocumentExists(command.Document))
                AddNotification("Document", "Este CPF já está em uso");
            // Verificar se E-mail já está cadastrado
            if(_repository.DocumentExists(command.Email))
                AddNotification("Email", "Este Email já está em uso");
            // Gerar os VOS
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode); 
          
            // Gerar as Entidades

            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));

            // Só muda a implementação do pagamento
            var payment = new PaypalPayment(
                command.TransactionCode,
                command.PaidDate,
                command.ExpireDate,
                command.Total,
                command.TotalPaid,
                command.Payer,
                address,
                email,
                new Document(command.PayerDocument,command.PayerDocumentType));

            // Relacionamentos
            subscription.AddPayment(payment);    
            student.AddSubscription(subscription);


            // Agrupar as validações

            AddNotifications(name, document, email, address, student, subscription, payment);

            // Salvar as informações
            _repository.CreateSubscription(student);

            // Enviar E-mail de boas vindas
            _emailService.Send(student.Name.ToString(), student.Email.Address, "Incrição realizada com sucesso", "Seja bem vindo a nossa plataforma. Sua assinatura foi criada com sucesso");

            // Retornar informações
            return new CommandResult(true, "Assinatura cadastrada com sucesso");
        }
    }
}